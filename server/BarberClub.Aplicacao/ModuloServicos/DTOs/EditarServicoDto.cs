namespace BarberClub.Aplicacao.ModuloServicos.DTOs;

public record EditarServicoDto(
    string titulo,
    decimal valor,
    int? duracao,
    bool isPromocao,
    int? porcentagemPromocao);