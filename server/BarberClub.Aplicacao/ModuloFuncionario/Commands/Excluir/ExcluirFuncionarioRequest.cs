using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.Excluir;

public record ExcluirFuncionarioRequest(Guid id) : IRequest<Result<ExcluirFuncionarioResponse>>;