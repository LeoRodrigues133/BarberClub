namespace BarberClub.Aplicacao.ModuloConfiguracao.DTOs;

public record AtualizarHorarioPorIdDto(
    TimeSpan? HoraAbertura,
    TimeSpan? HoraFechamento,
    bool Fechado);