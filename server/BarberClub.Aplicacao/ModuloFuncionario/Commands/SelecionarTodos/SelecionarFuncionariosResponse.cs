
using BarberClub.Aplicacao.ModuloFuncionario.DTOs;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;

public record SelecionarFuncionariosResponse
{
    public required IEnumerable<SelecionarFuncionariosDto> Funcionarios { get; init; }
}