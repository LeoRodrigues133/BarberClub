using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloServico;
using FluentResults;
using FluentValidation;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.Cadastrar;

public class CadastroServicoRequestHandler(
    IRepositorioServico _repositorioServico,
    IRepositorioFuncionario _repositorioFuncionario,
    IValidator<Servico> _validator,
    IContextoPersistencia _context,
    ITenantProvider _provider)
    : IRequestHandler<CadastrarServicoRequest, Result<CadastrarServicoResponse>>
{
    public async Task<Result<CadastrarServicoResponse>> Handle(
         CadastrarServicoRequest request,
         CancellationToken cancellationToken)
    {

        var funcionarioId = _provider.FuncionarioId;

        var servico = new Servico(
            funcionarioId.Value,
            request.titulo,
            request.valor,
            request.duracao ?? 0,
            request.porcentagemPromocao,
            request.isPromocao)
        {
            UsuarioId = _provider.EmpresaId.Value,
        };

        var validationResult = await _validator.ValidateAsync(servico, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();
            return Result.Fail(ErrorsResult.BadRequestError(errors));
        }

        try
        {
            await _repositorioServico.CadastrarAsync(servico);

            await _context.GravarAsync();
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();
            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new CadastrarServicoResponse(servico.Id));
    }
}