using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AbrirDataEspecifica;

public class AbrirDataEspecificaRequestHandler(
    IRepositorioConfiguracao _repositorioConfiguracao,
    IContextoPersistencia _context,
    IMemoryCache _cache)
    : IRequestHandler<AbrirDataEspecificaRequest, Result<AbrirDataEspecificaResponse>>
{
    public async Task<Result<AbrirDataEspecificaResponse>> Handle(
        AbrirDataEspecificaRequest request, CancellationToken cancellationToken)
    {
        var configuracao = await _repositorioConfiguracao.SelecionarPorEmpresaIdComHorariosAsync(request.empresaId);

        if (configuracao is null)
            return Result.Fail("Configuração não encontrada");

        var dataEncontrada = configuracao.DatasEspecificasFechado.Any(d => d.Date == request.data.Date);

        if (dataEncontrada)
            configuracao.DatasEspecificasFechado.Remove(request.data.Date);

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

        return Result.Ok(new AbrirDataEspecificaResponse(
            configuracao.DatasEspecificasFechado
            .Select(data =>
                data.ToShortDateString()
                ))
            );
    }
}