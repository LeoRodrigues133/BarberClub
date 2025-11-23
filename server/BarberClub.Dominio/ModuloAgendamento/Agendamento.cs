using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorario;
using BarberClub.Dominio.ModuloServico;

namespace BarberClub.Dominio.ModuloAgendamento;

public class Agendamento : EntidadeBase
{

    public Agendamento()
    {
        Status = EnumStatusAgendamento.Agendado;
    }

    public Agendamento(
        Guid funcionarioId,
        Guid horarioDisponivelId,
        Guid servicoId,
        DateTime dataAgendamento)
    {
        FuncionarioId = funcionarioId;
        HorarioDisponivelId = horarioDisponivelId;
        ServicoId = servicoId;
        DataAgendamento = dataAgendamento;
    }

    //public Guid ClienteId { get; set; }   ainda não implementado
    public Guid FuncionarioId { get; set; }
    public Guid HorarioDisponivelId { get; set; }
    public Guid ServicoId { get; set; }
    public DateTime DataAgendamento { get; set; }
    public EnumStatusAgendamento Status { get; set; }


    //public Cliente Cliente { get; set; }  ainda não implementado
    public Funcionario Funcionario { get; set; }
    public Servico Servico { get; set; }
    public HorarioDisponivel HorarioDisponivel { get; set; }

    public void Cancelar() =>
        Status = EnumStatusAgendamento.Cancelado;

    public void Confirmar() =>
        Status = EnumStatusAgendamento.Confirmado;

}
