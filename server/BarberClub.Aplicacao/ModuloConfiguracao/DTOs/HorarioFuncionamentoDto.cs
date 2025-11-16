namespace BarberClub.Aplicacao.ModuloConfiguracao.DTOs;

public record HorarioFuncionamentoDto(
    Guid Id,
    string DiaSemana,
    TimeSpan? HoraAbertura,
    TimeSpan? HoraFechamento,
    bool Fechado);