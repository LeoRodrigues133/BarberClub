using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;

namespace BarberClub.Dominio.ModuloHorarioFuncionamento;

public class HorarioFuncionamento
{
    public Guid Id { get; set; }

    public HorarioFuncionamento()
    {
        Id = Guid.NewGuid();
    }
    public HorarioFuncionamento(
        Guid configuracaoEmpresaId,
        SemanaEnum diaSemana,
        TimeSpan? horaAbertura,
        TimeSpan? horaFechamento,
        bool fechado)
    {
        Id = Guid.NewGuid();
        ConfiguracaoEmpresaId = configuracaoEmpresaId;
        DiaSemana = diaSemana;
        HoraAbertura = horaAbertura;
        HoraFechamento = horaFechamento;
        Fechado = fechado;
    }

    public Guid ConfiguracaoEmpresaId { get; set; }
    public SemanaEnum DiaSemana { get; set; }
    public TimeSpan? HoraAbertura { get; set; }
    public TimeSpan? HoraFechamento { get; set; }
    public bool Fechado { get; set; }

    public ConfiguracaoEmpresa? ConfiguracaoEmpresa { get; set; }
}
