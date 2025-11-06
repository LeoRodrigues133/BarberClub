using BarberClub.Aplicacao.ModuloServicos.DTOs;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarTodos;

public record SelecionarServicosResponse
{
    public required IEnumerable<SelecionarServicosDto> Servicos { get; init; }

}