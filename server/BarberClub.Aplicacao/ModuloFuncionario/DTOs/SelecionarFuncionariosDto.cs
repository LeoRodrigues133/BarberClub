using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloFuncionario.DTOs;

public record SelecionarFuncionariosDto(
    Guid id,
    string nome,
    string nomeApresentacao,
    string cpf,
    EnumCargo cargo,
    string email,
    int tempoAtendimento,
    int tempoIntervalo);
