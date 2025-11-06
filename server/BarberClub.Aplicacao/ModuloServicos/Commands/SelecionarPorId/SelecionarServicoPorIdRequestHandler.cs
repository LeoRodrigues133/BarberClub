using BarberClub.Dominio.ModuloServico;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.SelecionarPorId;

public class SelecionarServicoPorIdRequestHandler(
    IRepositorioServico _repositorioServico)
    : IRequestHandler<SelecionarServicoPorIdRequest, Result<SelecionarServicoPorIdResponse>>
{
    public async Task<Result<SelecionarServicoPorIdResponse>> Handle(SelecionarServicoPorIdRequest request, CancellationToken cancellationToken)
    {
        var servicoSelecionado = await _repositorioServico.SelecionarPorIdAsync(request.id);

        if (servicoSelecionado is null)
            return Result.Fail("Registro não encontrado");

        var resposta = new SelecionarServicoPorIdResponse
            (
            servicoSelecionado.Id,
            servicoSelecionado.Titulo,
            servicoSelecionado.ValorFinal,
            servicoSelecionado.Duracao,
            servicoSelecionado.IsPromocao,
            servicoSelecionado.Ativo
            );

        return Result.Ok(resposta);
    }
}
