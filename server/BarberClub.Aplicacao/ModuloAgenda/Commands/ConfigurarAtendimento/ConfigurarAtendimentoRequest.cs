using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloAgenda.Commands.ConfigurarAtendimento;

public record ConfigurarAtendimentoRequest(
    Guid FuncionarioId,
    int TempoAtendimento,
    int IntervaloEntreAtendimento) :
    IRequest <Result<ConfigurarAtendimentoResponse>>;