using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloServico;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.Excluir;

public class ExcluirServicoRequestHandler(
    IRepositorioServico _repositorioServico,
    IContextoPersistencia _context)
: IRequestHandler<ExcluirServicoRequest, Result<ExcluirServicoResponse>>
{
    public async Task<Result<ExcluirServicoResponse>> Handle(
        ExcluirServicoRequest request, CancellationToken cancellationToken)
    {
        var servicoSeleciona = await _repositorioServico.SelecionarPorIdAsync(request.id);

        if(servicoSeleciona is null)
            return Result.Fail(ErrorsResult.NotFoundError(request.id));

        try
        {
            await _repositorioServico.ExcluirAsync(servicoSeleciona);

            await _context.GravarAsync();
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new ExcluirServicoResponse());
    }
}