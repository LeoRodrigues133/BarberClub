using BarberClub.Aplicacao.ModuloFuncionario.DTOs;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.CadastrarVariosHorarios;

public class CadastrarVariosHorariosRequestHandler(
    IRepositorioFuncionario _repositorioFuncionario,
    IRepositorioHorarioDisponivel _repositorioHorarioDisponivel,
    IRepositorioHorarioFuncionamento _horarioFuncionamento,
    IRepositorioConfiguracao _repositorioConfiguracao,
    IContextoPersistencia _context)
    : IRequestHandler<CadastrarVariosHorariosRequest, Result<CadastrarVariosHorariosResponse>>
{
    public async Task<Result<CadastrarVariosHorariosResponse>> Handle(
        CadastrarVariosHorariosRequest request, CancellationToken cancellationToken)
    {

        var funcionario = await _repositorioFuncionario.SelecionarPorIdAsync(request.funcionarioId);

        if (funcionario is null)
            return Result.Fail("Funcionário não encontrado");

        var configuracao = await _repositorioConfiguracao.SelecionarPorEmpresaIdComHorariosAsync(funcionario.AdminId);

        if(configuracao is null)
            return Result.Fail("Configuração não encontrada");


        var horariosFuncionamento = await _horarioFuncionamento
            .SelecionarTodosAsync();

        if (!horariosFuncionamento.Any())
            return Result.Fail("A empresa não possui horários de funcionamento configurados");

        try
        {
            var horariosNovos = funcionario
                .GerarHorariosDisponiveis(
                configuracao,
                horariosFuncionamento,
                request.mesSelecionado,
                request.anoSelecionado);

            if (!horariosNovos.Any())
                return Result.Fail("Nenhum horário disponível foi gerado. Verifique a configuração.");

            var todosExistentes = await _repositorioHorarioDisponivel.SelecionarTodosAsync();

            var horariosRelevantes = todosExistentes
                .Where(h =>
                    h.DataEspecifica.Month == request.mesSelecionado &&
                    h.DataEspecifica.Year == request.anoSelecionado)
                .ToList();

            var horariosParaInserir = horariosNovos
                .Where(n => !horariosRelevantes.Any(e => ConferirDuplicata(n, e)))
                .ToList();


            var idsGerados = await _repositorioHorarioDisponivel.CadastrarVariosAsync(horariosParaInserir);

            foreach (var id in horariosRelevantes)
                await _repositorioHorarioDisponivel.EditarAsync(id);

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
    bool ConferirDuplicata(HorarioDisponivel novo, HorarioDisponivel exist)
    {
        return exist.DataEspecifica.Date == novo.DataEspecifica.Date
               && exist.HorarioInicio == novo.HorarioInicio;
    }
}
