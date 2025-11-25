using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.SelecionarHorariosPorData;

public record SelecionarHorariosPorDataRequest(
    DateTime dataSelecionada)
    : IRequest<Result<SelecionarHorariosPorDataResponse>>;