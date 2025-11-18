using BarberClub.Aplicacao.ModuloFuncionario.Commands.Cadastrar;
using BarberClub.Aplicacao.ModuloFuncionario.Commands.Editar;
using BarberClub.Aplicacao.ModuloFuncionario.Commands.Excluir;
using BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;
using BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;
using BarberClub.Aplicacao.ModuloFuncionario.DTOs;
using BarberClub.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberClub.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/funcionario")]
public class FuncionarioController(IMediator _mediator) : ControllerBase
{
    // POST: api/funcionario/cadastrar
    [HttpPost("cadastrar")]
    [ProducesResponseType(typeof(CadastrarFuncionarioResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cadastrar(CadastrarFuncionarioRequest request)
    {
        var result = await _mediator.Send(request);
        return result.ToHttpResponse();

    }

    // PUT: api/funcionario/editar/{id}
    [HttpPut("editar/{id:guid}")]
    [ProducesResponseType(typeof(EditarFuncionarioResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarFuncionarioDto request)
    {
        var editarRequest = new EditarFuncionarioRequest(
            id,
            request.nome,
            request.cpf,
            request.cargo);

        var result = await _mediator.Send(editarRequest);

        return result.ToHttpResponse();

    }
    
    // DELETE: api/funcionario/excluir/{id}
    [HttpDelete("excluir/{id:guid}")]
    [ProducesResponseType(typeof(ExcluirFuncionarioResponse),StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var request = new ExcluirFuncionarioRequest(id);

        var result = await _mediator.Send(request);

        return result.ToHttpResponse();
    }
    // GET: api/funcionario
    [HttpGet]
    [ProducesResponseType(typeof(SelecionarFuncionariosDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var result = await _mediator.Send(new SelecionarFuncionariosRequest());
        return result.ToHttpResponse();

    }
    // GET: api/funcionario/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarFuncionarioPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarFuncionarioPorIdRequest(id);

        var result = await _mediator.Send(selecionarPorIdRequest);

        return result.ToHttpResponse();

    }
}