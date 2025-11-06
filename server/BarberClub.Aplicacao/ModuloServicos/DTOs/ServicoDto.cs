namespace BarberClub.Aplicacao.ModuloServicos.DTOs;

public record ServicoDto(
    string titulo,
    decimal valor,
    bool? isPromocao,
    int porcentagemPromocao,
    int durcaoo);
