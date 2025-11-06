using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarPorId;

public record SelecionarServicoPorIdRequest(Guid id)
    : IRequest<Result<SelecionarServicoPorIdResponse>>;