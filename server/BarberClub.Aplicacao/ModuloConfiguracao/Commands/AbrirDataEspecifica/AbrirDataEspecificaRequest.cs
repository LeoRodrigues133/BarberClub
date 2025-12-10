using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AbrirDataEspecifica;

public record AbrirDataEspecificaRequest(Guid empresaId, DateTime data)
    : IRequest <Result<AbrirDataEspecificaResponse>>;