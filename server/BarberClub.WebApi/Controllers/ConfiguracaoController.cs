using MediatR;
using Microsoft.AspNetCore.Mvc;
using BarberClub.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarBanner;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarLogo;
using BarberClub.Aplicacao.ModuloConfiguracao.DTOs;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarNomeEmpresa;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.CarregarConfiguracao;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarHorárioPorId;
using static BarberClub.Aplicacao.Compartilhado.AzureBlobService;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.FecharDataEspecifica;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.AbrirDataEspecifica;

namespace BarberClub.WebApi.Controllers;

[ApiController]
[Authorize(Roles = "Administrador")]
[Route("api/configuracao")]
[Produces("application/json")]
public class ConfiguracaoController(IMediator _mediator, IAzureBlobService _azureBlobService) : ControllerBase
{

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CarregarConfiguracaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterConfiguracao()
    {
        var empresaId = Guid.Empty;

        empresaId = User.GetEmpresaId();

        if (empresaId == Guid.Empty)
        {
            empresaId = User.GetUserId();
        }

        var query = new CarregarConfiguracaoRequest(empresaId);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.ToHttpResponse());

        return result.ToHttpResponse();
    }

    [HttpPost("gerar-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GerarTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GerarUrlComToken([FromBody] GerarTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Url))
            return BadRequest(new { Erro = "URL é obrigatória" });

        try
        {
            var blobName = ExtrairNomeBlob(request.Url);

            var urlComToken = await _azureBlobService.GerarUrlComToken(
                blobName,
                TimeSpan.FromDays(1)
            );

            return Ok(new GerarTokenResponse(urlComToken));
        }
        catch (UriFormatException)
        {
            return BadRequest(new { Erro = "URL inválida" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Erro = $"Erro ao gerar token: {ex.Message}" });
        }
    }

    [HttpPut("banner")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(5_242_880)]
    [ProducesResponseType(typeof(AtualizarBannerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarBanner([FromForm] AtualizarArquivoDto request)
    {
        if (request.arquivo == null || request.arquivo.Length == 0)
            return BadRequest(new { Erro = "Arquivo é obrigatório" });

        var empresaId = User.GetUserId();
        var editarRequest = new AtualizarBannerRequest(empresaId, request.arquivo);

        var result = await _mediator.Send(editarRequest);

        if (result.IsFailed)
            return BadRequest(result.ToHttpResponse());

        return result.ToHttpResponse();
    }

    [HttpPut("logo")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(5_242_880)]
    [ProducesResponseType(typeof(AtualizarLogoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarLogo([FromForm] AtualizarArquivoDto request)
    {
        if (request.arquivo == null || request.arquivo.Length == 0)
            return BadRequest(new { Erro = "Arquivo é obrigatório" });

        var empresaId = User.GetUserId();
        var editarRequest = new AtualizarLogoRequest(empresaId, request.arquivo);

        var result = await _mediator.Send(editarRequest);

        if (result.IsFailed)
            return BadRequest(result.ToHttpResponse());

        return result.ToHttpResponse();
    }

    [HttpPut("nome")]
    [ProducesResponseType(typeof(AtualizarNomeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarNome([FromBody] AtualizarNomeDto request)
    {
        if (string.IsNullOrWhiteSpace(request.nomeEmpresa))
            return BadRequest(new { Erro = "Nome da empresa é obrigatório" });

        if (request.nomeEmpresa.Length < 3)
            return BadRequest(new { Erro = "Nome da empresa deve ter no mínimo 3 caracteres" });

        var empresaId = User.GetUserId();
        var editarRequest = new AtualizarNomeRequest(empresaId, request.nomeEmpresa);

        var result = await _mediator.Send(editarRequest);

        if (result.IsFailed)
            return BadRequest(result.ToHttpResponse());

        return result.ToHttpResponse();
    }

    [HttpPut("horario/{id:guid}")]
    [ProducesResponseType(typeof(AtualizarHorarioPorIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarHorarioPorId(Guid id, AtualizarHorarioPorIdDto request)
    {
        var atualizarHorario = new AtualizarHorarioPorIdRequest(
            id,
            request.HoraAbertura,
            request.HoraFechamento,
            request.Fechado);

        var result = await _mediator.Send(atualizarHorario);

        return result.ToHttpResponse();
    }

    [HttpPut("fechar-data")]
    [ProducesResponseType(typeof(FecharDataEspecificaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FecharDataSelecionada(DataEspecificaDto request)
    {
        var empresaId = Guid.Empty;

        empresaId = User.GetEmpresaId();

        var dataRequest = new FecharDataEspecificaRequest(
            empresaId,
            request.Data);

        var result = await _mediator.Send(dataRequest);

        return result.ToHttpResponse();
    }

    [HttpPut("abrir-data")]
    [ProducesResponseType(typeof(AbrirDataEspecificaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AbrirDataSelecionada(DataEspecificaDto request)
    {
        var empresaId = Guid.Empty;

        empresaId = User.GetEmpresaId();

        var dataRequest = new AbrirDataEspecificaRequest(
            empresaId,
            request.Data);

        var result = await _mediator.Send(dataRequest);

        return result.ToHttpResponse();
    }

    private static string ExtrairNomeBlob(string url)
    {
        var uri = new Uri(url);

        var segments = uri.Segments.Skip(2);

        return string.Join("", segments);
    }
}
