using BarberClub.Dominio.ModuloHorarioFuncionamento;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.CadastrarVariosHorarios;

public record CadastrarVariosHorariosRequest(
    Guid funcionarioId)
    : IRequest<Result<CadastrarVariosHorariosResponse>>;