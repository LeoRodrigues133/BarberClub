using MediatR;
using FluentResults;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarHorárioPorId;
using BarberClub.Dominio.ModuloHorarioFuncionamento;
using Microsoft.Extensions.Caching.Memory;
using BarberClub.Dominio.ModuloConfiguracao;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarHorarioPorId;

public class AtualizarHorariaPorIdRequestHandler(
    IRepositorioHorarioFuncionamento _repositorioHorarioFuncionamento,
    IRepositorioConfiguracao _repositorioConfiguracao,
    IMemoryCache _cache,
    IContextoPersistencia _context)
    : IRequestHandler<AtualizarHorarioPorIdRequest, Result<AtualizarHorarioPorIdResponse>>
{
    public async Task<Result<AtualizarHorarioPorIdResponse>> Handle(
        AtualizarHorarioPorIdRequest request, CancellationToken cancellationToken)
    {
        var horarioSelecionado = await _repositorioHorarioFuncionamento.SelecionarPorIdAsync(request.Id);

        if (horarioSelecionado is null)
            return Result.Fail("Horario não encontrado");

        var configuracao = await _repositorioConfiguracao
            .SelecionarPorIdAsync(horarioSelecionado.ConfiguracaoEmpresaId);

        if(configuracao is null)
            return Result.Fail("Configuração não encontrado");

        AtualizarHorarioSelecionado(request, horarioSelecionado);

        try
        {
            await _repositorioHorarioFuncionamento.EditarAsync(horarioSelecionado);

            _cache.Remove($"configuracao-{configuracao.UsuarioId}");


            await _context.GravarAsync();
        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();

            return Result.Fail(ex.Message);
        }

        return Result.Ok(new AtualizarHorarioPorIdResponse(horarioSelecionado.Id));
    }

    private void AtualizarHorarioSelecionado(
        AtualizarHorarioPorIdRequest request,
        HorarioFuncionamento horarioSelecionado)
    {
        if (request.Fechado)
        {
            horarioSelecionado.Fechado = true;
            horarioSelecionado.HoraAbertura = TimeSpan.Zero;
            horarioSelecionado.HoraFechamento = TimeSpan.Zero;
            return;
        }

        horarioSelecionado.Fechado = false;

        if (request.HoraAbertura.HasValue)
            horarioSelecionado.HoraAbertura = request.HoraAbertura.Value;

        if (request.HoraFechamento.HasValue)
            horarioSelecionado.HoraFechamento = request.HoraFechamento.Value;
    }
}
