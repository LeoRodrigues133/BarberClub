using BarberClub.Aplicacao.ModuloServicos.DTOs;
using BarberClub.Dominio.ModuloServico;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarTodos;

public class SelecionarServicosRequestHandler(
    IRepositorioServico _repositorioServico)
    : IRequestHandler<SelecionarServicosRequest, Result<SelecionarServicosResponse>>
{
    public async Task<Result<SelecionarServicosResponse>> Handle(
        SelecionarServicosRequest request, CancellationToken cancellationToken)
    {

        var registros = await _repositorioServico.SelecionarTodosAsync();

        var servicos = new List<SelecionarServicosDto>();

 
        var resposta = new SelecionarServicosResponse
        {
            Servicos = registros
            .Select(x => new SelecionarServicosDto(
                x.Id,
                x.Titulo,
                x.Valor,
                x.Duracao ?? 0
            )).ToList()
        };
        return Result.Ok(resposta);
    }
}