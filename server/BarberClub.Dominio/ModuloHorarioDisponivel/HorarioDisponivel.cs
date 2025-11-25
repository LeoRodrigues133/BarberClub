using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgendamento;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public bool PodeDesativar(DateTime data)
    {
        if (!Ativo)
            return false;

        if (data != DataEspecifica)
            return false;

        return !Agendamentos.Any(a =>
            a.DataAgendamento.Date == data.Date &&
            a.Status != EnumStatusAgendamento.Cancelado);
    }
    public bool PodeAtivar(DateTime data)
    {
        if (Ativo)
            return false;

        if (data != DataEspecifica)
            return false;

        return !Agendamentos.Any(a =>
            a.DataAgendamento.Date == data.Date &&
            a.Status == EnumStatusAgendamento.Agendado ||
            a.Status == EnumStatusAgendamento.Cancelado);
    }
}