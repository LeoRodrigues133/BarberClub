using BarberClub.Dominio.ModuloHorarioFuncionamento;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.CadastrarVariosHorarios;

public record CadastrarVariosHorariosRequest(
    Guid funcionarioId,
    int mesSelecionado,
    int anoSelecionado)
    : IRequest<Result<CadastrarVariosHorariosResponse>>;