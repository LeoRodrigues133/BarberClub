using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.Excluir;

public record ExcluirServicoRequest(Guid id) : IRequest<Result<ExcluirServicoResponse>>;