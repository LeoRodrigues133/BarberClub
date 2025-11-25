namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.ConfigurarAtendimento;

public record ConfigurarAtendimentoResponse(Guid funcionarioId, int TempoAtendimento, int TempoIntervalo);