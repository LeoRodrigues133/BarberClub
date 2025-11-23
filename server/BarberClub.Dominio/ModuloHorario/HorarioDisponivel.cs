using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgendamento;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;

namespace BarberClub.Dominio.ModuloHorario;

public class HorarioDisponivel : EntidadeBase
{

    public HorarioDisponivel()
    {
        Agendamentos = new List<Agendamento>();
    }

    public HorarioDisponivel(
        Guid funcionarioId,
        Guid horarioFuncionamentoId,
        SemanaEnum diaSemana,
        TimeSpan horarioInicio,
        TimeSpan horarioFim,
        bool ativo,
        DateTime dataEspecifica)
    {
        FuncionarioId = funcionarioId;
        HorarioFuncionamentoId = horarioFuncionamentoId;
        DiaSemana = diaSemana;
        HorarioInicio = horarioInicio;
        HorarioFim = horarioFim;
        Ativo = ativo;
        DataEspecifica = dataEspecifica;
    }

    public Guid FuncionarioId { get; set; }
    public Guid HorarioFuncionamentoId { get; set; }
    public SemanaEnum DiaSemana { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFim { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataEspecifica { get; set; }

    public Funcionario Funcionario { get; set; }
    public HorarioFuncionamento HorarioFuncionamento { get; set; }
    public List<Agendamento> Agendamentos { get; set; }

    public bool EstaOcupadoPorData(DateTime dataEspecifica) =>
         Agendamentos.Any(a => a.DataAgendamento.Date == dataEspecifica &&
         a.Status != EnumStatusAgendamento.Cancelado);

}