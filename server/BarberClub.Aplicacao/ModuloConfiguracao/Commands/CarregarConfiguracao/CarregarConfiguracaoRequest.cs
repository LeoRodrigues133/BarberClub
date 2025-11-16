using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.CarregarConfiguracao;
public record ObterConfiguracaoQuery(Guid EmpresaId)
    : IRequest<Result<ConfiguracaoEmpresaResponse>>;
