namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;

public record SelecionarFuncionarioPorIdResponse(Guid id, string nome, string cpf, string cargo, int tempoAtendimento, int tempoIntervalo);