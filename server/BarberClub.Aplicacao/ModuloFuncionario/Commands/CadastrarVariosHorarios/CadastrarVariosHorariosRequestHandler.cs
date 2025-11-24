using BarberClub.Aplicacao.ModuloFuncionario.DTOs;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.CadastrarVariosHorarios;

public class CadastrarVariosHorariosRequestHandler(
    IRepositorioFuncionario _repositorioFuncionario,
    IRepositorioHorarioDisponivel _repositorioHorarioDisponivel,
    IRepositorioHorarioFuncionamento _horarioFuncionamento,
    IContextoPersistencia _context)
    : IRequestHandler<CadastrarVariosHorariosRequest, Result<CadastrarVariosHorariosResponse>>
{
    public async Task<Result<CadastrarVariosHorariosResponse>> Handle(
        CadastrarVariosHorariosRequest request, CancellationToken cancellationToken)
    {

        var funcionario = await _repositorioFuncionario.SelecionarPorIdAsync(request.funcionarioId);

        if (funcionario == null)
            return Result.Fail("Funcionário não encontrado");

        var horariosFuncionamento = await _horarioFuncionamento
            .SelecionarTodosAsync();

        if (!horariosFuncionamento.Any())
            return Result.Fail("A empresa não possui horários de funcionamento configurados");

        try
        {
            var horariosNovos = funcionario
                .GerarHorariosDisponiveis(horariosFuncionamento);

            if (!horariosNovos.Any())
                return Result.Fail("Nenhum horário disponível foi gerado. Verifique a configuração.");

            var idsGerados = await _repositorioHorarioDisponivel.CadastrarVariosAsync(horariosNovos);

            await _context.GravarAsync();
            var res = new CadastrarVariosHorariosResponse(
              qtHorariosGerados: horariosNovos.Count,
              HorariosCadastrados: horariosNovos
                  .GroupBy(h => h.DiaSemana)
                  .SelectMany(g => g.Take(3))
                  .Select(h => new HorarioCadastradoDto(
                      h.DiaSemana,
                      h.HorarioInicio,
                      h.HorarioFim))
                  .ToList()
              );

            return Result.Ok(res);
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ex.ToString());
        }
    }
}
