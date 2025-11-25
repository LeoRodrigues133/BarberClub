using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.AtivarHorario;

public record AtivarHorarioRequest(Guid horarioId)
    : IRequest<Result<AtivarHorarioResponse>>;