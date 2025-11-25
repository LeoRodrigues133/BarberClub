using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.ConfigurarAtendimento;

public record ConfigurarAtendimentoRequest(Guid id, int tempoAtendimento, int tempoIntervalo)
    : IRequest<Result<ConfigurarAtendimentoResponse>>;