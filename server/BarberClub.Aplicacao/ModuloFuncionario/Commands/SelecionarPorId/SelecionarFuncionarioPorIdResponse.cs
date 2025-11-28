namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;

public record SelecionarFuncionarioPorIdResponse(Guid id, string nome, string cpf, string cargo,string email, int tempoAtendimento, int tempoIntervalo);