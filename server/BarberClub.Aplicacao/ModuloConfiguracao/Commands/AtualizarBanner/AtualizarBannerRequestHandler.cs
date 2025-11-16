using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarBanner;

public class AtualizarBannerRequestHandler(
    IMemoryCache _cache,
    IContextoPersistencia _context,
    IAzureBlobService _azureBlobService,
    IRepositorioConfiguracao _repositorioConfiguracao
    )
    : IRequestHandler<AtualizarBannerRequest, Result<AtualizarBannerResponse>>
{
    public async Task<Result<AtualizarBannerResponse>> Handle(
        AtualizarBannerRequest request, CancellationToken cancellationToken)
    {

        var configuracaoSelecionada =
            await _repositorioConfiguracao.SelecionarPorEmpresaIdComHorariosAsync(request.empresaId);

        if (configuracaoSelecionada is null)
            return Result.Fail("Configuração não encontrada");

        var destino = "Empresas/Banner";

        var uploadBanner = await _azureBlobService
            .UploadAsync(request.empresaId, request.arquivo, destino);

        configuracaoSelecionada.BannerUrl = uploadBanner;

        try
        {
            await _repositorioConfiguracao.EditarAsync(configuracaoSelecionada);

            _cache.Remove($"configuracao-{request.empresaId}");

            await _context.GravarAsync();
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new AtualizarBannerResponse(uploadBanner));
    }
}