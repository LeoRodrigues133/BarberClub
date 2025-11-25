using BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;
using BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.AtivarHorario;
using BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.DesativarHorario;
using BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.SelecionarHorariosPorData;
using BarberClub.Aplicacao.ModuloHorarioDisponivel.DTOs;
using BarberClub.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberClub.WebApi.Controllers;

[ApiController]
[Authorize(Roles = ("Administrador, Funcionario"))]
[Route("api/horario")]
public class HorarioDisponivelController(IMediator _mediator) : ControllerBase
{

    [HttpPut("desativar/{horarioId:guid}")]
    [ProducesResponseType(typeof(DesativarHorarioResponse),StatusCodes.Status200OK)]
    public async Task<IActionResult> DesativarHorario(Guid horarioId)
    {
        var desativarRequest = new DesativarHorarioRequest(horarioId);
        var result = await _mediator.Send(desativarRequest);

        return result.ToHttpResponse();
    }

    [HttpPut("ativar/{horarioId:guid}")]
    [ProducesResponseType(typeof(AtivarHorarioResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AtivarHorario(Guid horarioId)
    {
        var ativarRequest = new AtivarHorarioRequest(horarioId);
        var result = await _mediator.Send(ativarRequest);

        return result.ToHttpResponse();
    }

    [HttpPost("visualizar-por-data/{id:guid}")]
    [ProducesResponseType(typeof(SelecionarHorariosPorDataResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarHorariosPorData(Guid id, SelecionarHorariosPorDataRequest request)
    {
        var result = await _mediator.Send(request);

        return result.ToHttpResponse();
    }

}