using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;

public record SelecionarFuncionariosRequest : IRequest<Result<SelecionarFuncionariosResponse>>;