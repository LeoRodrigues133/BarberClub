using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.ConfigurarAtendimento;

public record ConfigurarAtendimentoRequest(Guid id, int tempoAtendimento, int tempoIntervalo)
    : IRequest<Result<ConfigurarAtendimentoResponse>>;