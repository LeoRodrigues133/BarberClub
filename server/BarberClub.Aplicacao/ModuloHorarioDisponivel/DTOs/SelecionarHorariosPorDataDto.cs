using BarberClub.Dominio.ModuloAgendamento;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.DTOs;

public record SelecionarHorariosPorDataDto(
    Guid id,
    TimeSpan horario,
    bool status
    );