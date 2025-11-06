using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.Excluir;

public class ExcluirFuncionarioRequestHandler(
    IRepositorioFuncionario _repositorioFuncionario,
    IContextoPersistencia _contextoPersistencia
    )
    : IRequestHandler<ExcluirFuncionarioRequest, Result<ExcluirFuncionarioResponse>>
{
    public async Task<Result<ExcluirFuncionarioResponse>> Handle(
        ExcluirFuncionarioRequest request, CancellationToken cancellationToken)
    {
        var funcionarioSelecionado = await _repositorioFuncionario.SelecionarPorIdAsync(request.id);

        if (funcionarioSelecionado is null)
            return Result.Fail("Registro não encontrado");

        try
        {
            await _repositorioFuncionario.ExcluirAsync(funcionarioSelecionado);

            await _contextoPersistencia.GravarAsync();
        }
        catch (Exception ex)
        {
            await _contextoPersistencia.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new ExcluirFuncionarioResponse());
    }
}