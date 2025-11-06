using BarberClub.Aplicacao.ModuloAutenticacao.Commands.Autenticar;
using BarberClub.Aplicacao.ModuloAutenticacao.Commands.Registrar;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BarberClub.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator _mediator, SignInManager<Usuario> signInManager) : ControllerBase
{
    // POST: api/auth/registrar
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(RegistrarUsuarioRequest request)
    {

        var tokenResult = await _mediator.Send(request);
        return tokenResult.ToHttpResponse();

    }

    // POST: api/auth/autenticar
    [HttpPost("autenticar")]
    public async Task<IActionResult> Autenticar(AutenticarUsuarioRequest request)
    {
        var tokenResult = await _mediator.Send(request);
        return tokenResult.ToHttpResponse();

    }

    // POST: api/auth/sair
    [HttpPost("sair")]
    [Authorize]
    public async Task<IActionResult> Sair()
    {
        await signInManager.SignOutAsync();
        return Ok("Logout realizado com sucesso!");
    }
}
