using BarberClub.Aplicacao.ModuloAutenticacao.DTOs;
using BarberClub.Dominio.ModuloAutenticacao;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BarberClub.Aplicacao.ModuloAutenticacao.Commands.Autenticar;

public class AutenticarUsuarioRequestHandler(
    SignInManager<Usuario> _signInManager,
    UserManager<Usuario> _userManager,
    ITokenProvider _tokenProvider)
    : IRequestHandler<AutenticarUsuarioRequest, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(
        AutenticarUsuarioRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _signInManager.PasswordSignInAsync(
            request.userName,
            request.password,
            isPersistent: false,
            lockoutOnFailure: false //Corrigir
            );

        var usuario = await _userManager.FindByNameAsync(request.userName);

        if (usuario is null)
            return Result.Fail(new Error("Usuário não encontrado")); //Corrigir

        if (authResult.IsLockedOut)
            return Result.Fail(new Error("A conta foi bloqueada por excesso de tentativas")); //Corrigir

        if (authResult.IsNotAllowed)
            if (!await _userManager.IsEmailConfirmedAsync(usuario) && usuario.Email is not null)
                return Result.Fail(new Error("O email requisitado ainda não foi confirmado")); //Corrigir

        if (!authResult.Succeeded)
            return Result.Fail(new Error("Usuário ou senha incorretos")); //Corrigir

        var tokenAcesso = _tokenProvider.GerarAccessToken(usuario) as TokenResponse;

        if(tokenAcesso is null)
            return Result.Fail(new Error("Falha ao gerar token")); //Corrigir

        return Result.Ok(tokenAcesso);
    }
}