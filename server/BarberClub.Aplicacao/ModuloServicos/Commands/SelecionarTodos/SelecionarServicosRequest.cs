using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarTodos;

public record SelecionarServicosRequest : IRequest<Result<SelecionarServicosResponse>>;