using BarberClub.Aplicacao.ModuloFuncionario.DTOs;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgendamento;
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

        if (configuracao is null)
            return Result.Fail("Configuração não encontrada");


        var horariosFuncionamento = await _horarioFuncionamento
            .SelecionarTodosAsync();

        if (!horariosFuncionamento.Any())
            return Result.Fail("A empresa não possui horários de funcionamento configurados");


        var horariosNovos = funcionario
            .GerarHorariosDisponiveis(
            configuracao,
            horariosFuncionamento,
            request.mesSelecionado,
            request.anoSelecionado);

        if (!horariosNovos.Any())
            return Result.Fail("Nenhum horário disponível foi gerado. Verifique a configuração.");

        var horariosExistentes = await _repositorioHorarioDisponivel
            .SelecionarPorFuncionarioEPeriodoAsync(
            request.funcionarioId,
            request.mesSelecionado,
            request.anoSelecionado);

        var horariosParaManter = horariosExistentes.Where(existente =>
        {
            bool temAgendamentoAtivo = existente.Agendamentos.Any(a =>
                a.Status == EnumStatusAgendamento.Agendado ||
                a.Status == EnumStatusAgendamento.Confirmado);

            if (temAgendamentoAtivo)
                return true;

            bool coincideComNovo = horariosNovos.Any(novo =>
                novo.DataEspecifica.Date == existente.DataEspecifica.Date &&
                novo.HorarioInicio == existente.HorarioInicio);

            return coincideComNovo;
        }).ToList();

        var horariosParaRemover = horariosExistentes
            .Except(horariosParaManter)
            .ToList();

        var horariosParaInserir = horariosNovos
            .Where(novo => !horariosExistentes.Any(existente =>
                existente.DataEspecifica.Date == novo.DataEspecifica.Date &&
                existente.HorarioInicio == novo.HorarioInicio))
            .ToList();

        try
        {
            foreach (var horario in horariosParaRemover)
                await _repositorioHorarioDisponivel.ExcluirAsync(horario.Id);


            var idsGerados = await _repositorioHorarioDisponivel.CadastrarVariosAsync(horariosParaInserir);

            await _context.GravarAsync();

            var totalHorariosProcessados = horariosParaInserir.Count;
            var totalHorariosRemovidos = horariosParaRemover.Count;


            var res = new CadastrarVariosHorariosResponse(
              qtHorariosGerados: totalHorariosProcessados,
              qtHorariosRemovidos: totalHorariosRemovidos,
              HorariosCadastrados: horariosNovos
                  .GroupBy(h => h.DiaSemana)
                    .OrderBy(g => g.Key)
                    .SelectMany(g =>
                    g.OrderBy(h => h.HorarioInicio)
                    .Take(horariosNovos.Count))
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
