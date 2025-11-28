using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;

public record SelecionarFuncionarioPorIdResponse(
    Guid id,
    string nome,
    string cpf,
    EnumCargo cargo,
    string email,
    int tempoAtendimento,
    int tempoIntervalo);