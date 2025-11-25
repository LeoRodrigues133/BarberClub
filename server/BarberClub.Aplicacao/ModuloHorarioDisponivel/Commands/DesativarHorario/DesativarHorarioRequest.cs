using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.DesativarHorario;

public record DesativarHorarioRequest(Guid horarioId)
    : IRequest<Result<DesativarHorarioResponse>>;