using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarConfiguracao;

public class AtualizarConfiguracaoRequestHandler(
    IRepositorioConfiguracao _repositorioConfiguracao,
        IMemoryCache _cache,
        IContextoPersistencia _context)
    : IRequestHandler<AtualizarConfiguracaoRequest, Result<AtualizarConfiguracaoResponse>>
{
    public async Task<Result<AtualizarConfiguracaoResponse>> Handle(
        AtualizarConfiguracaoRequest request, CancellationToken cancellationToken)
    {

        var configuracao = await _repositorioConfiguracao
            .SelecionarPorEmpresaIdComHorariosAsync(request.adminId);

        if (configuracao is null)
            return Result.Fail("Configuração não encontrada");

        if (!string.IsNullOrEmpty(request.nomeEmpresa))
            configuracao!.NomeEmpresa = request.nomeEmpresa;

        if (request.horarios != null && request.horarios.Any())
            foreach (var horarioDto in request.horarios)
            {
                var horario = configuracao.HorarioDeExpediente
                    .FirstOrDefault(h => h.Id == horarioDto.Id);

                if (horario != null)
                {
                    horario.HoraAbertura = horarioDto.HoraAbertura;
                    horario.HoraFechamento = horarioDto.HoraFechamento;
                    horario.Fechado = horarioDto.Fechado;
                }
            }

        try
        {
            await _repositorioConfiguracao.EditarAsync(configuracao);

            var cacheKey = $"configuracao-{request.adminId}";
            _cache.Remove(cacheKey);

            await _context.GravarAsync();

        }
        catch (Exception ex)
        {
            await _context.DesfazerAsync();
            return Result.Fail(ErrorsResult.InternalServerError(ex));

        }
        return Result.Ok(new AtualizarConfiguracaoResponse(configuracao.Id));
    }
}