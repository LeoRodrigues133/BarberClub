using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloFuncionario.DTOs;

public record SelecionarFuncionariosDto(
    Guid id,
    string nome,
    string cpf,
    string cargo,
    string email,
    int tempoAtendimento,
    int tempoIntervalo);
