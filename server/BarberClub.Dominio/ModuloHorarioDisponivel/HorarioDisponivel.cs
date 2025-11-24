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
        SemanaEnum diaSemana,
        TimeSpan horarioInicio,
        TimeSpan horarioFim,
        bool ativo,
        DateTime dataEspecifica)
    {
        FuncionarioId = funcionarioId;
        DiaSemana = diaSemana;
        HorarioInicio = horarioInicio;
        HorarioFim = horarioFim;
        Ativo = ativo;
        DataEspecifica = dataEspecifica;
    }

    public Guid FuncionarioId { get; set; }
    public SemanaEnum DiaSemana { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFim { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataEspecifica { get; set; }

    public Funcionario? Funcionario { get; set; } // navegação
    public List<Agendamento> Agendamentos { get; set; }

    public void Desativar()
    {
        if (Agendamentos.Any(a =>
            a.Status == EnumStatusAgendamento.Agendado ||
            a.Status == EnumStatusAgendamento.Confirmado))
        {
            throw new InvalidOperationException(
                "Não pode desativar horário com agendamentos ativos");
        }

        Ativo = false;
    }

    public void Ativar() => Ativo = true;

    public bool EstaDisponivel(DateTime data)
    {
        if (!Ativo)
            return false;

        var diaSemana = (SemanaEnum)((int)data.DayOfWeek);
        if (diaSemana != DiaSemana)
            return false;

        return !Agendamentos.Any(a =>
            a.DataAgendamento.Date == data.Date &&
            a.Status != EnumStatusAgendamento.Cancelado);
    }
}