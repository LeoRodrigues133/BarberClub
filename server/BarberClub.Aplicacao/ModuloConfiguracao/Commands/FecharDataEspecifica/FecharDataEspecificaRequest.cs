using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.FecharDataEspecifica;

public record FecharDataEspecificaRequest(Guid empresaId, DateTime data)
    : IRequest<Result<FecharDataEspecificaResponse>>;