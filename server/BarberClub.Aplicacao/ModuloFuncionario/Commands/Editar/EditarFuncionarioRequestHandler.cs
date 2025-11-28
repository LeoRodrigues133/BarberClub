using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloFuncionario;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.Editar;

public class EditarFuncionarioRequestHandler(
    IRepositorioFuncionario _repositorioFuncionario,
    IContextoPersistencia _contextoPersistencia,
    UserManager<Usuario> _userManager,
    RoleManager<Cargo> _roleManager,
    IValidator<Funcionario> _validator)
    : IRequestHandler<EditarFuncionarioRequest, Result<EditarFuncionarioResponse>>
{
    public async Task<Result<EditarFuncionarioResponse>> Handle(
        EditarFuncionarioRequest request, CancellationToken cancellationToken)
    {
        var funcionarioSelecionado = await _repositorioFuncionario.SelecionarPorIdAsync(request.id);

        if (funcionarioSelecionado is null)
            return Result.Fail(ErrorsResult.NotFoundError(request.id));

        await ValidacaoFuncionario(request, funcionarioSelecionado);

        var validationResult = await _validator.ValidateAsync(funcionarioSelecionado);

        if(!validationResult.IsValid)
        {
            var errors = validationResult.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ErrorsResult.BadRequestError(errors));
        }

        if (request.cargo.HasValue)
        {
            var rolesAtuais = await _userManager.GetRolesAsync(funcionarioSelecionado.Usuario);
            if (rolesAtuais.Any())
                await _userManager.RemoveFromRolesAsync(funcionarioSelecionado.Usuario, rolesAtuais);

            var nomeCargo = request.cargo.Value.ToString();
            await _userManager.AddToRoleAsync(funcionarioSelecionado.Usuario, nomeCargo);
        }

        try
        {
            await _repositorioFuncionario.EditarAsync(funcionarioSelecionado);

            await _contextoPersistencia.GravarAsync();
        }
        catch (Exception ex)
        {
            await _contextoPersistencia.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new EditarFuncionarioResponse(funcionarioSelecionado.Id));
    }

    private async Task<Result> ValidacaoFuncionario(EditarFuncionarioRequest request, Funcionario funcionarioSelecionado)
    {
        if (funcionarioSelecionado.Usuario is null)
            return Result.Fail("Usuário associado não encontrado");

        if (!string.IsNullOrEmpty(request.nome))
            funcionarioSelecionado.Nome = request.nome;

        if (!string.IsNullOrEmpty(request.cpf))
        {
            var cpfDisponivel = await VerificarCpf(request.cpf);

            if (!cpfDisponivel)
                return Result.Fail("CPF já cadastrado para outro funcionário");

            funcionarioSelecionado.Cpf = request.cpf;

        }

        if (!string.IsNullOrEmpty(request.email))
            funcionarioSelecionado.Usuario.Email = request.email;

        return Result.Ok();
    }
    private async Task<bool> VerificarCpf(string cpf)
    {
        if (await _repositorioFuncionario.ExistePorCpfAsync(cpf))
            return false;

        return true;

    }

}