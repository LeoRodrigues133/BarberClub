using BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;
using FluentResults;
using MediatR;

namespace BarberClub.WebApi.Controllers;
public record SelecionarFuncionarioPorIdRequest(Guid id)
    : IRequest<Result<SelecionarFuncionarioPorIdResponse>>;