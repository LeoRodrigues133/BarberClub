using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloHorarioFuncionamento;

namespace BarberClub.Dominio.ModuloConfiguracao;

public class ConfiguracaoEmpresa : EntidadeBase
{
    public ConfiguracaoEmpresa()
    {
        Ativo = true;
        DataCriacao = DateTime.UtcNow;
        HorarioDeExpediente = new List<HorarioFuncionamento>();
        DatasEspecificasFechado = new List<DateTime>();
    }
    public ConfiguracaoEmpresa(
        Guid usuarioId,
        string nomeEmpresa,
        string logoUrl,
        string bannerUrl)
    {
        UsuarioId = usuarioId;
        NomeEmpresa = nomeEmpresa;
        LogoUrl = logoUrl;
        BannerUrl = bannerUrl;

    }

    public string NomeEmpresa { get; set; }
    public string LogoUrl { get; set; }
    public string BannerUrl { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }

    public List<HorarioFuncionamento> HorarioDeExpediente { get; set; }
    public List<DateTime> DatasEspecificasFechado { get; set; }

    public bool isAberto(List<HorarioFuncionamento> horarios)
    {
        var hoje = (SemanaEnum)((int)DateTime.Now.DayOfWeek);
        var horarioHoje = horarios.FirstOrDefault(x => x.DiaSemana == hoje);

        if (horarioHoje == null || horarioHoje.Fechado)
            return false;

        var agora = TimeOnly.FromDateTime(DateTime.Now);

        if (!horarioHoje.HoraAbertura.HasValue || !horarioHoje.HoraFechamento.HasValue)
            return false;

        var abertura = TimeOnly.FromTimeSpan(horarioHoje.HoraAbertura.Value);
        var fechamento = TimeOnly.FromTimeSpan(horarioHoje.HoraFechamento.Value);

        return agora >= abertura && agora <= fechamento;
    }
}