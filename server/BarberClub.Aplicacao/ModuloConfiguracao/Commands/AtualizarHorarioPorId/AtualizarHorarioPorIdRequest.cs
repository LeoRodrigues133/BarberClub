using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarHorárioPorId;

public record AtualizarHorarioPorIdRequest(
    Guid Id,
    TimeSpan? HoraAbertura,
    TimeSpan? HoraFechamento,
    bool Fechado
    )
    : IRequest <Result<AtualizarHorarioPorIdResponse>>;