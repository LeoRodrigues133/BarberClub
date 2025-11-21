using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.CarregarConfiguracao;
public record CarregarConfiguracaoRequest(Guid EmpresaId)
    : IRequest<Result<CarregarConfiguracaoResponse>>;
