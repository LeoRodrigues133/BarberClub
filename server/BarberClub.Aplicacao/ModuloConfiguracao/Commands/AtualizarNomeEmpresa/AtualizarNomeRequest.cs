using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarNomeEmpresa;

public record AtualizarNomeRequest(Guid empresaId, string nomeEmpresa)
    : IRequest<Result<AtualizarNomeResponse>>;