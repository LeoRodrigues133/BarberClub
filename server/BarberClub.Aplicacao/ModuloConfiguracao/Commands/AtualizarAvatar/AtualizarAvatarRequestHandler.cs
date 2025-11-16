using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using BarberClub.Dominio.ModuloFuncionario;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarAvatar;

public class AtualizarAvatarRequestHandler(
    IMemoryCache _cache,
    IAzureBlobService _azureBlobService,
    IContextoPersistencia _context,
    IRepositorioFuncionario _repositorioFuncionario)
    : IRequestHandler<AtualizarAvatarRequest, Result<AtualizarAvatarResponse>>
{
    public async Task<Result<AtualizarAvatarResponse>> Handle(
        AtualizarAvatarRequest request, CancellationToken cancellationToken)
    {
        var funcionarioSelecionado =
            await _repositorioFuncionario.SelecionarPorIdAsync(request.funcionarioId);

        if (funcionarioSelecionado == null)
            return Result.Fail("Funcionario não encontrado");

        var destino = "Funcionarios/Avatar";

        var uploudAvatar = await _azureBlobService
            .UploadAsync(request.funcionarioId, request.arquivo, destino);

        //funcionarioSelecionado.urlAvatar = uploudAvatar; //corrigir, ainda não existe.

        try
        {
            await _repositorioFuncionario.EditarAsync(funcionarioSelecionado);

            _cache.Remove($"configuracao-{request.funcionarioId}");

            await _context.GravarAsync();
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new AtualizarAvatarResponse(uploudAvatar));
    }
}
