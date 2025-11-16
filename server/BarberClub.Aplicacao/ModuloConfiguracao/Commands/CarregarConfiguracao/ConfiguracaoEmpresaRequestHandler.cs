using BarberClub.Aplicacao.ModuloConfiguracao.DTOs;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.CarregarConfiguracao;


public class ObterConfiguracaoQueryHandler(
        IMemoryCache _cache,
        IRepositorioConfiguracao _repositorioConfiguracao
    )
    : IRequestHandler<ObterConfiguracaoQuery, Result<ConfiguracaoEmpresaResponse>>
{

    public async Task<Result<ConfiguracaoEmpresaResponse>> Handle(
        ObterConfiguracaoQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"configuracao-{request.EmpresaId}";

        var configuracao = await _repositorioConfiguracao
            .SelecionarPorEmpresaIdComHorariosAsync(request.EmpresaId);

        if (configuracao is null)
            return Result.Fail("Configuração não encontrada");

        var response = new ConfiguracaoEmpresaResponse(
            configuracao.Id,
            configuracao.NomeEmpresa,
            configuracao.LogoUrl,
            configuracao.BannerUrl,
            configuracao.Ativo,
            configuracao.DataCriacao,
            configuracao.HorarioDeExpediente
                .Select(h => new HorarioFuncionamentoDto(
                    h.Id,
                    h.DiaSemana.ToString(),
                    h.HoraAbertura,
                    h.HoraFechamento,
                    h.Fechado
                ))
                .ToList()
        );

        _cache.Set(
            cacheKey,
            response,
            TimeSpan.FromMinutes(15));

        return Result.Ok(response);
    }
}