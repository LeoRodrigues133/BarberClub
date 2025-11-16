using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarBanner;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarLogo;

public class AtualizarLogoRequestHandler(
    IMemoryCache _cache,
    IContextoPersistencia _context,
    IAzureBlobService _azureBlobService,
    IRepositorioConfiguracao _repositorioConfiguracao)
    : IRequestHandler<AtualizarLogoRequest, Result<AtualizarLogoResponse>>
{
    public async Task<Result<AtualizarLogoResponse>> Handle(
        AtualizarLogoRequest request, CancellationToken cancellationToken)
    {
        var configuracaoSelecionada = await _repositorioConfiguracao.SelecionarPorEmpresaIdComHorariosAsync(request.empresaId);

        if (configuracaoSelecionada is null)
            return Result.Fail("Configuração não encontrada");

        var destino = "Empresas/Logo";

        var uploudLogo = await _azureBlobService
            .UploadAsync(request.empresaId, request.arquivo, destino);

        configuracaoSelecionada.LogoUrl = uploudLogo;

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

        return Result.Ok(new AtualizarLogoResponse(uploudLogo));
    }
}