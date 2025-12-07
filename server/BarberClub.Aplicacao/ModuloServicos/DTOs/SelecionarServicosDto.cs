namespace BarberClub.Aplicacao.ModuloServicos.DTOs;

public record SelecionarServicosDto(
    Guid id,
    Guid funcionarioId,
    string titulo,
    decimal valor,
    decimal valorFinal,
    int? porcentagemPromocional,
    int? duracao,
    bool ativo);