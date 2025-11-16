using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarNomeEmpresa;

public class AtualizarNomeRequestHandler(
    IRepositorioConfiguracao _repositorioConfiguracao,
    IContextoPersistencia _context,
    IMemoryCache _cache)
    : IRequestHandler<AtualizarNomeRequest, Result<AtualizarNomeResponse>>
{
    public async Task<Result<AtualizarNomeResponse>> Handle(
        AtualizarNomeRequest request, CancellationToken cancellationToken)
    {
        var configuracaoSelecionada =
            await _repositorioConfiguracao.SelecionarPorEmpresaIdComHorariosAsync(request.empresaId);

        if (configuracaoSelecionada is null)
            return Result.Fail("Configuração não encontrada.");

        configuracaoSelecionada.NomeEmpresa = request.nomeEmpresa;

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

        return Result.Ok(new AtualizarNomeResponse(configuracaoSelecionada.NomeEmpresa));
    }
}