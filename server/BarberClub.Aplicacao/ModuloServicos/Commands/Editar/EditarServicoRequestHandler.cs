using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloServico;
using FluentResults;
using FluentValidation;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.Editar;

public class EditarServicoRequestHandler(
    IRepositorioServico _repositorioServico,
    IValidator<Servico> _validator,
    IContextoPersistencia _context
    )
    : IRequestHandler<EditarServicoRequest, Result<EditarServicoResponse>>
{
    public async Task<Result<EditarServicoResponse>> Handle(
        EditarServicoRequest request, CancellationToken cancellationToken)
    {
        var servicoSelecionado = await _repositorioServico.SelecionarPorIdAsync(request.id);

        if (servicoSelecionado is null)
            return Result.Fail(ErrorsResult.NotFoundError(request.id));

        ValidarDados(request, servicoSelecionado);

        var validationResult = await _validator.ValidateAsync(servicoSelecionado);

        if (!validationResult.IsValid)
        {
            var Errors = validationResult.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorsResult.BadRequestError(Errors));
        }

        try
        {
            await _repositorioServico.EditarAsync(servicoSelecionado);

            await _context.GravarAsync();
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));

        }

        return Result.Ok(new EditarServicoResponse(servicoSelecionado.Id));
    }

    private static void ValidarDados(EditarServicoRequest request, Servico servicoSelecionado)
    {
        if (!string.IsNullOrEmpty(request.titulo))
            servicoSelecionado.Titulo = request.titulo;

        if (request.valor > 0)
            servicoSelecionado.Valor = request.valor;

        if (request.duracao > 10)
            servicoSelecionado.Duracao = request.duracao;
    }
}