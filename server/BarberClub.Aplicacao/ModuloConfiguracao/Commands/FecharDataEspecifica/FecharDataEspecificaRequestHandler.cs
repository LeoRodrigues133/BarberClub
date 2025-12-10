using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.FecharDataEspecifica;

public class FecharDataEspecificaRequestHandler(
    IRepositorioConfiguracao _repositorioConfiguracao,
    IContextoPersistencia _context,
    IMemoryCache _cache)
    : IRequestHandler<FecharDataEspecificaRequest, Result<FecharDataEspecificaResponse>>
{
    public async Task<Result<FecharDataEspecificaResponse>> Handle(
        FecharDataEspecificaRequest request, CancellationToken cancellationToken)
    {

        var configuracao = await _repositorioConfiguracao.SelecionarPorEmpresaIdComHorariosAsync(request.empresaId);

        if (configuracao is null)
            return Result.Fail("Configuração não encontrada");

        var datasEncontradas = configuracao.DatasEspecificasFechado.Any(d => d.Date == request.data.Date);

        if (!datasEncontradas)
            configuracao.DatasEspecificasFechado.Add(request.data.Date);

        try
        {
            await _repositorioConfiguracao.EditarAsync(configuracao);

            _cache.Remove($"configuracao-{request.empresaId}");

            await _context.GravarAsync();
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new FecharDataEspecificaResponse(
            configuracao.DatasEspecificasFechado
            .Select(data =>
                data.ToShortDateString()
                ))
            );
    }
}