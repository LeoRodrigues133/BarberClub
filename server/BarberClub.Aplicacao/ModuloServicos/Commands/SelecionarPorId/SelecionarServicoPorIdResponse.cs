namespace BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarPorId;

public record SelecionarServicoPorIdResponse(
    Guid id,
    string titulo,
    decimal valorFinal,
    int? duracao,
    bool isPromocao,
    bool ativo);