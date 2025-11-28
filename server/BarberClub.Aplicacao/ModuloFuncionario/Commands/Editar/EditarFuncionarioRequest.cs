using BarberClub.Dominio.ModuloAutenticacao;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.Editar;

public record EditarFuncionarioRequest(Guid id, string? nome, string? cpf, string? email, EnumCargo? cargo)
    : IRequest<Result<EditarFuncionarioResponse>>;