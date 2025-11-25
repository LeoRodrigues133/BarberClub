using BarberClub.Aplicacao.ModuloHorarioDisponivel.DTOs;
namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.SelecionarHorariosPorData;

public record SelecionarHorariosPorDataResponse
{
    public required IEnumerable<SelecionarHorariosPorDataDto> Horarios { get; init; }
}