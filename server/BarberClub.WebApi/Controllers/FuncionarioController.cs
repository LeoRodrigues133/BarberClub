using BarberClub.Aplicacao.ModuloFuncionario.Commands.Cadastrar;
using BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;
using BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;
using BarberClub.Aplicacao.ModuloFuncionario.DTOs;
using BarberClub.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BarberClub.WebApi.Controllers;

[ApiController]
[Route("api/funcionario")]
public class FuncionarioController(IMediator _mediator) : ControllerBase
{
    // POST: api/funcionario/registrar
    [HttpPost("cadastrar")]
    public async Task<IActionResult> Registrar(CadastrarFuncionarioRequest request)
    {
        var tokenResult = await _mediator.Send(request);
        return tokenResult.ToHttpResponse();

    }

    // GET: api/funcionario
    [HttpGet]
    [ProducesResponseType(typeof(SelecionarFuncionariosDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var result = await _mediator.Send(new SelecionarFuncionariosRequest());
        return result.ToHttpResponse();

    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarFuncionarioPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarFuncionarioPorIdRequest(id);

        var result = await _mediator.Send(selecionarPorIdRequest);

        return result.ToHttpResponse();
    }
}