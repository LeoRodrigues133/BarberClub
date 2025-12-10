using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Aplicacao.ModuloAutenticacao.DTOs;
using BarberClub.Aplicacao.ModuloConfiguracao.Services;
using BarberClub.Dominio.ModuloAutenticacao;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BarberClub.Aplicacao.ModuloAutenticacao.Commands.Registrar;

public class RegistrarUsuarioRequestHandler(
    RoleManager<Cargo> _roleManager,
    UserManager<Usuario> _userManager,
    ITokenProvider _tokenProvider,
    ServiceConfiguracao _serviceConfiguracao)
    : IRequestHandler<RegistrarUsuarioRequest, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(
        RegistrarUsuarioRequest request, CancellationToken cancellationToken)
    {
        var emailCadastrado = await _userManager.FindByEmailAsync(request.email);

        if (emailCadastrado is not null)
            return Result.Fail("Email já cadastrado");


        var usuario = new Usuario
        {
            UserName = request.userName,
            Email = request.email,
            NomeApresentacao = request.nomeApresentacao
        };

        var usuarioResult = await _userManager.CreateAsync(usuario, request.password);

        if (!usuarioResult.Succeeded)
        {
            var errors = usuarioResult
                .Errors
                .Select(failure => failure.Description)
                .ToList();

            return Result.Fail(ErrorsResult.BadRequestError(errors));
        }

        const string cargoAdmin = nameof(EnumCargo.Administrador);

        if (!await _roleManager.RoleExistsAsync(cargoAdmin))
        {
            var createRoleResult = await _roleManager.CreateAsync(new Cargo (EnumCargo.Administrador));
            if (!createRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(usuario);

                var errors = createRoleResult
                    .Errors
                    .Select(e => e.Description)
                    .ToList();

                return Result.Fail(ErrorsResult.InternalServerError(
                    new Exception($"Criação de cargo falhou: {string.Join("; ", errors)}")));
            }
        }

        var addRoleResult = await _userManager.AddToRoleAsync(usuario, cargoAdmin);

        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(usuario);

            var errors = addRoleResult
                .Errors
                .Select(e => e.Description)
                .ToList();

            return Result.Fail(ErrorsResult.InternalServerError(
                new Exception($"Falha ao atribuir cargo: {string.Join("; ", errors)}")));
        }

        var resultConfig = await _serviceConfiguracao.CriarConfiguracaoPadraoAsync(usuario.Id, request.nomeEmpresa);

        if (resultConfig.IsFailed)
        {
            await _userManager.DeleteAsync(usuario);
            var errors = resultConfig.Errors.Select(e => e.Message).ToList();
            return Result.Fail(ErrorsResult.BadRequestError(errors));
        }

        var tokenAcesso = _tokenProvider.GerarAccessToken(usuario) as TokenResponse;

        if (tokenAcesso is null)
            return Result.Fail(ErrorsResult.InternalServerError(
                new Exception("Falha ao gerar token de acesso")));

        return Result.Ok(tokenAcesso);
    }
}
