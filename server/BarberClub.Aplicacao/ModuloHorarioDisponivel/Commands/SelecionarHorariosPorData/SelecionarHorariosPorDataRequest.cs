using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.SelecionarHorariosPorData;

public record SelecionarHorariosPorDataRequest(
    Guid funcionarioId,
    DateTime dataSelecionada)
    : IRequest<Result<SelecionarHorariosPorDataResponse>>;