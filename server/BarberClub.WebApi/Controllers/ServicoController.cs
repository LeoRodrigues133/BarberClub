using BarberClub.Aplicacao.ModuloFuncionario.Commands.Excluir;
using BarberClub.Aplicacao.ModuloServicos.Commands.Cadastrar;
using BarberClub.Aplicacao.ModuloServicos.Commands.Editar;
using BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarPorId;
using BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarTodos;
using BarberClub.Aplicacao.ModuloServicos.DTOs;
using BarberClub.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberClub.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/servico")]
public class ServicoController(IMediator _mediator) : ControllerBase
{
    // POST: api/servico/cadastrar
    [HttpPost("cadastrar")]
    [ProducesResponseType(typeof(CadastrarServicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cadastrar(CadastrarServicoRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.ToHttpResponse());
    }

    // PUT: api/servico/editar/{id}
    [HttpPut("editar/{id:guid}")]
    [ProducesResponseType(typeof(EditarServicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarServicoRequest request)
    {
        var editarRequest = new EditarServicoRequest(
            id,
            request.titulo,
            request.valor,
            request.duracao);

        var result = await _mediator.Send(editarRequest);

        return Ok(result.ToHttpResponse());
    }

    // DELETE: api/servico/excluir/{id}
    [HttpDelete("excluir/{id:guid}")]
    [ProducesResponseType (typeof(ExcluirFuncionarioResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var request = new ExcluirFuncionarioRequest(id);

        var result = await _mediator.Send(request);

        return Ok(result.ToHttpResponse());
    }

    // GET: api/servico
    [HttpGet]
    [ProducesResponseType(typeof(SelecionarServicosDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var result = await _mediator.Send(new SelecionarServicosRequest());
        return Ok(result.ToHttpResponse());
    }

    // GET: api/servico/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarServicoPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarServicoPorIdRequest(id);

        var result = await _mediator.Send(selecionarPorIdRequest);

        return Ok(result.ToHttpResponse());
    }
}