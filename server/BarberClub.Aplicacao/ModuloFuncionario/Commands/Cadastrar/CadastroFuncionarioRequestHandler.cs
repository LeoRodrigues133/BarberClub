using MediatR;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloFuncionario;
using FluentValidation;


namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.Cadastrar;

public class CadastroFuncionarioRequestHandler(
    ITenantProvider _tenantProvider,
    RoleManager<Cargo> _roleManager,
    UserManager<Usuario> _userManager,
    IValidator<Funcionario> _validator,
    IRepositorioFuncionario _repositorioFuncionario,
    IContextoPersistencia _contextoPersistencia)
    : IRequestHandler<CadastrarFuncionarioRequest, Result<CadastrarFuncionarioResponse>>
{
    public async Task<Result<CadastrarFuncionarioResponse>> Handle(
        CadastrarFuncionarioRequest request, CancellationToken cancellationToken)
    {

        var validacaoResult = await ValidarAutorizacaoAsync(request.cargo);

        var adminLogado = validacaoResult.Value;

        var validarEmailECpf = await ValidarCredenciais(request.email, request.cpf);

        if (!validarEmailECpf.IsSuccess)
            return Result.Fail(validarEmailECpf.Errors);

        var usuarioResult = await CriarUsuarioAsync(request);

        if (usuarioResult.IsFailed)
            return Result.Fail(usuarioResult.Errors);

        var usuario = usuarioResult.Value;

        var cargoResult = await AtribuirCargoAsync(usuario, request.cargo);

        if (cargoResult.IsFailed)
        {
            await _userManager.DeleteAsync(usuario);
            return Result.Fail(cargoResult.Errors);
        }

        var funcionario = new Funcionario(request.nome, request.cpf)
        {
            UsuarioId = usuario.Id,
            AdminId = adminLogado.Id
        };

        var validationResult = await _validator.ValidateAsync(funcionario);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            await _userManager.DeleteAsync(usuario);

            return Result.Fail(ErrorsResult.BadRequestError(errors));
        }

        try
        {
            await _repositorioFuncionario.CadastrarAsync(funcionario);
            await _contextoPersistencia.GravarAsync();
        }
        catch (Exception ex)
        {
            await _contextoPersistencia.DesfazerAsync();
            await _userManager.DeleteAsync(usuario);
            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }


        return Result.Ok(new CadastrarFuncionarioResponse(funcionario.Id));
    }

    private async Task<Result> AtribuirCargoAsync(Usuario usuario, EnumCargo cargo)
    {
        var cargoNome = cargo.ToString();

        if (!await _roleManager.RoleExistsAsync(cargoNome))
            await _roleManager.CreateAsync(new Cargo { Name = cargoNome });

        var roleResultado = await _userManager.AddToRoleAsync(usuario, cargoNome);

        if (!roleResultado.Succeeded)
        {
            await _userManager.DeleteAsync(usuario);
            return Result.Fail("Falha ao atribuir cargo.");
        }

        return Result.Ok();
    }

    private async Task<Result<Usuario>> CriarUsuarioAsync(CadastrarFuncionarioRequest request)
    {
        var usuario = new Usuario
        {
            UserName = request.userName,
            Email = request.email,
            NomeApresentacao = string.IsNullOrEmpty(request.nomeApresentacao) ? request.nome : request.nomeApresentacao,
        };

        var resultado = await _userManager.CreateAsync(usuario, request.password);

        if (!resultado.Succeeded)
        {
            var erros = resultado.Errors.Select(e => e.Description);
            return Result.Fail(string.Join("; ", erros));
        }

        return Result.Ok(usuario);
    }

    private async Task<Result> ValidarCredenciais(string email, string cpf)
    {
        var emailExiste = await _userManager.FindByEmailAsync(email);

        if (emailExiste is not null)
            return Result.Fail("Email já cadastrado");

        if (await _repositorioFuncionario.ExistePorCpfAsync(cpf))
            return Result.Fail("O Cpf já está cadastrado");

        return Result.Ok();
    }

    private async Task<Result<Usuario>> ValidarAutorizacaoAsync(EnumCargo cargo)
    {
        if (cargo is EnumCargo.Administrador)
            return Result.Fail("Não é permitido criar um novo Administrador.");

        var adminId = _tenantProvider.EmpresaId;

        if (adminId is null)
            return Result.Fail("Usuário não Autenticado.");

        var adminLogado = await _userManager.FindByIdAsync(adminId.ToString()!);

        if (adminLogado is null)
            return Result.Fail("Usuário não encontrado.");

        var isAdmin = await _userManager
            .IsInRoleAsync(adminLogado, EnumCargo.Administrador.ToString());

        if (!isAdmin)
            return Result.Fail("Apenas administradores podem criar funcionários");

        return Result.Ok(adminLogado);
    }
}
