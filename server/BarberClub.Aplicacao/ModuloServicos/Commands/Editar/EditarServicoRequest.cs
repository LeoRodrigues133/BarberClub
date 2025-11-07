using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.Editar;

public record EditarServicoRequest(
    Guid id,
    string titulo,
    decimal valor,
    int duracao) : IRequest<Result<EditarServicoResponse>>;