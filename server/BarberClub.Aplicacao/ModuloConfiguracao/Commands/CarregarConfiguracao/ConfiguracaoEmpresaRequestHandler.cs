using BarberClub.Aplicacao.ModuloConfiguracao.DTOs;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.CarregarConfiguracao;


public class CarregarConfiguracaoRequestHandler(
        IMemoryCache _cache,
        IRepositorioConfiguracao _repositorioConfiguracao,
        IAzureBlobService _azureBlobService
    )
    : IRequestHandler<CarregarConfiguracaoRequest, Result<CarregarConfiguracaoResponse>>
{

    public async Task<Result<CarregarConfiguracaoResponse>> Handle(
        CarregarConfiguracaoRequest request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"configuracao-{request.EmpresaId}";

        if (_cache.TryGetValue<CarregarConfiguracaoResponse>(cacheKey, out var cachedResponse))
            return Result.Ok(cachedResponse!);

        var configuracao = await _repositorioConfiguracao
            .SelecionarPorEmpresaIdComHorariosAsync(request.EmpresaId);

        if (configuracao is null)
            return Result.Fail("Configuração não encontrada");

        string? logoUrlComToken = null;
        string? bannerUrlComToken = null;

        try
        {
            if (!string.IsNullOrWhiteSpace(configuracao.LogoUrl))
            {
                logoUrlComToken = await _azureBlobService.GerarUrlComToken(
                    configuracao.LogoUrl,
                    TimeSpan.FromDays(1)
                );
            }

            if (!string.IsNullOrWhiteSpace(configuracao.BannerUrl))
            {
                bannerUrlComToken = await _azureBlobService.GerarUrlComToken(
                    configuracao.BannerUrl,
                    TimeSpan.FromDays(1)
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gerar tokens SAS: {ex.Message}");

            logoUrlComToken = configuracao.LogoUrl;
            bannerUrlComToken = configuracao.BannerUrl;
        }

        var response = new CarregarConfiguracaoResponse(
            configuracao.Id,
            configuracao.NomeEmpresa,
            logoUrlComToken,
            bannerUrlComToken,
            configuracao.Ativo,
            configuracao.DataCriacao,
            configuracao.HorarioDeExpediente
                .OrderBy(x => x.DiaSemana)
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
            TimeSpan.FromDays(1));

        return Result.Ok(response);
    }
}