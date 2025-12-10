using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using BarberClub.Dominio.ModuloHorarioFuncionamento;
using FluentResults;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Services;

public class ServiceConfiguracao(
    IRepositorioConfiguracao _repositorioConfiguracao,
    IContextoPersistencia _context)
{
    public async Task<Result<ConfiguracaoEmpresa>> CriarConfiguracaoPadraoAsync(Guid usuarioId, string nomeEmpresa)
    {
        var config = await _repositorioConfiguracao
            .SelecionarPorEmpresaIdComHorariosAsync(usuarioId);

        if (config is null)
        {
            config = new ConfiguracaoEmpresa()
            {
                UsuarioId = usuarioId,
                NomeEmpresa = nomeEmpresa,
                BannerUrl = "",
                LogoUrl = ""
            };

            var horarioPadrao = CriarHorariosPadrao(config.Id);
            config.HorarioDeExpediente.AddRange(horarioPadrao);

            await _repositorioConfiguracao.CadastrarConfiguracaoInicialAsync(config);

            await _context.GravarAsync();
        }

        return Result.Ok(config);
    }

    private List<HorarioFuncionamento> CriarHorariosPadrao(Guid configuracaoEmpresaId)
    {
        return new List<HorarioFuncionamento>
        {
            new(
                configuracaoEmpresaId,
                SemanaEnum.Domingo,
                TimeSpan.Zero,
                TimeSpan.Zero,
                fechado: true),
            new(
                configuracaoEmpresaId,
                SemanaEnum.Segunda,
                new TimeSpan(8, 0, 0),
                new TimeSpan(18, 0, 0),
                fechado:false),
            new(
                configuracaoEmpresaId,
                SemanaEnum.Terca,
                new TimeSpan(8, 0, 0),
                new TimeSpan(18, 0, 0),
                fechado:false),
            new(
                configuracaoEmpresaId,
                SemanaEnum.Quarta,
                new TimeSpan(8, 0, 0),
                new TimeSpan(18, 0, 0),
                fechado:false),
            new(
                configuracaoEmpresaId,
                SemanaEnum.Quinta,
                new TimeSpan(8, 0, 0),
                new TimeSpan(18, 0, 0),
                fechado:false),
            new(
                configuracaoEmpresaId,
                SemanaEnum.Sexta,
                new TimeSpan(8, 0, 0),
                new TimeSpan(18, 0, 0),
                fechado:false),
            new(
                configuracaoEmpresaId,
                SemanaEnum.Sabado,
                new TimeSpan(8, 0, 0),
                new TimeSpan(14, 0, 0),
                fechado:false)
        };
    }
}