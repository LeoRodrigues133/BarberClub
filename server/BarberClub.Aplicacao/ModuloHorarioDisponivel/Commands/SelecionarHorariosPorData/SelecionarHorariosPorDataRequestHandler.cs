using BarberClub.Aplicacao.ModuloHorarioDisponivel.DTOs;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorario;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.SelecionarHorariosPorData;

public class SelecionarHorariosPorDataRequestHandler(
    IRepositorioHorarioDisponivel _repositorioHorario)
    : IRequestHandler<SelecionarHorariosPorDataRequest, Result<SelecionarHorariosPorDataResponse>>
{
    public async Task<Result<SelecionarHorariosPorDataResponse>> Handle(
        SelecionarHorariosPorDataRequest request, CancellationToken cancellationToken)
    {
        var todosOsHorarios = await _repositorioHorario
            .SelecionarTodosAsync();

        var horariosDoDia = todosOsHorarios
            .Where(x => x.DataEspecifica == request.dataSelecionada
                    && x.FuncionarioId == request.funcionarioId)
            .ToList();

        var resposta = new SelecionarHorariosPorDataResponse
        {
            Horarios = horariosDoDia
            .Select(x => new SelecionarHorariosPorDataDto(
                x.Id,
                x.HorarioInicio,
                x.Ativo
            ))
            .OrderBy(x => x.horario)
            .ToList()
        };

        return Result.Ok(resposta);
    }
}