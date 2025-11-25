using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.ConfigurarAtendimento;

public class ConfigurarAtendimentoRequestHandler(
    IRepositorioFuncionario _repositorioFuncionario,
    IContextoPersistencia _context)
    : IRequestHandler<ConfigurarAtendimentoRequest, Result<ConfigurarAtendimentoResponse>>
{
    public async Task<Result<ConfigurarAtendimentoResponse>> Handle(
        ConfigurarAtendimentoRequest request, CancellationToken cancellationToken)
    {
        var funcionarioSelecionado = await _repositorioFuncionario.SelecionarPorIdAsync(request.id);

        if (funcionarioSelecionado is null)
            return Result.Fail("Registro não encontrado");

        funcionarioSelecionado.AtualizarTempoAtendimento(request.tempoAtendimento, request.tempoIntervalo);

        try
        {
            await _repositorioFuncionario.EditarAsync(funcionarioSelecionado);
            await _context.GravarAsync();
        }
        catch(Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ErrorsResult.InternalServerError(ex));
        }

        return Result.Ok(new ConfigurarAtendimentoResponse(
            funcionarioSelecionado.Id,
            funcionarioSelecionado.TempoAtendimento,
            funcionarioSelecionado.TempoIntervalo));
    }
}