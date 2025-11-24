namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.ConfigurarAtendimento;

public record ConfigurarAtendimentoResponse(Guid funcionarioId, int TempoAtendimento, int TempoIntervalo);