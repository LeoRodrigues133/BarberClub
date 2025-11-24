using BarberClub.Dominio.ModuloHorarioFuncionamento;

namespace BarberClub.Aplicacao.ModuloFuncionario.DTOs;

public record HorarioCadastradoDto
(
    SemanaEnum DiaSemana,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFim);