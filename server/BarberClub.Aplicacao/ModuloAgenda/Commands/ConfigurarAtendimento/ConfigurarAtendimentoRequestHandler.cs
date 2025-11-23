using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgenda;
using BarberClub.Dominio.ModuloFuncionario;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloAgenda.Commands.ConfigurarAtendimento;

public class ConfigurarAtendimentoRequestHandler(
    IRepositorioAgenda _repositorioAgenda,
    IContextoPersistencia _contextoPersistencia
    )
    : IRequestHandler<ConfigurarAtendimentoRequest, Result<ConfigurarAtendimentoResponse>>
{
    public async Task<Result<ConfigurarAtendimentoResponse>> Handle(
        ConfigurarAtendimentoRequest request, CancellationToken cancellationToken)
    {
        var configuracaoSelecionada = await _repositorioAgenda.SelecionarPorFuncionarioId(request.FuncionarioId);

        if (configuracaoSelecionada is null)
            return Result.Fail("Configuração não encontrada");

        configuracaoSelecionada.TempoAtendimento = request.TempoAtendimento;
        configuracaoSelecionada.IntervaloEntreAtendimento = request.IntervaloEntreAtendimento;

        try
        {
            await _repositorioAgenda.EditarAsync(configuracaoSelecionada);
            await _contextoPersistencia.GravarAsync();
        }
        catch (Exception ex)
        {

            await _contextoPersistencia.DesfazerAsync();

            return Result.Fail(ex.ToString());
        }

        return Result.Ok(new ConfigurarAtendimentoResponse(configuracaoSelecionada.Id));
    }
}
