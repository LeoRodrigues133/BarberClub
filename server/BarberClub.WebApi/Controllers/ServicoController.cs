using BarberClub.Aplicacao.ModuloServicos.Commands.Cadastrar;
using BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarTodos;
using BarberClub.Aplicacao.ModuloServicos.DTOs;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloServico;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using BarberClub.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/servico")]
public class ServicoController(IMediator _mediator) : ControllerBase
{
    // POST: api/servico/cadastrar
    [HttpPost]
    [ProducesResponseType (typeof(CadastrarServicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cadastrar(CadastrarServicoRequest request)
    {
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
    [HttpGet("debug")]
    public async Task<IActionResult> Debug(
    [FromServices] ITenantProvider tenant,
    [FromServices] BarberClubDbContext ctx)
    {
        var funcId = tenant.FuncionarioId;

        var servicos = await ctx.Set<Servico>()
            .IgnoreQueryFilters()
            .Select(s => new { s.Id, s.Titulo, s.FuncionarioId })
            .ToListAsync();

        return Ok(new
        {
            meuFuncionarioId = funcId,
            todosServicos = servicos,
            servicosQueSaoMeus = servicos.Where(s => s.FuncionarioId == funcId)
        });
    }
}