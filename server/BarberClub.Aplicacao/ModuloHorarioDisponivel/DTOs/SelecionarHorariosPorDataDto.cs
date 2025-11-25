using BarberClub.Dominio.ModuloAgendamento;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.DTOs;

public record SelecionarHorariosPorDataDto(
    TimeSpan horario,
    bool status
    );