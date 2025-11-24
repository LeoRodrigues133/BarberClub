using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgendamento;
using BarberClub.Dominio.ModuloConfiguracao;
using BarberClub.Dominio.ModuloHorario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;

namespace BarberClub.Dominio.ModuloFuncionario;

public class Funcionario : EntidadeBase
{

    public Funcionario()
    {
        HorarioDisponivels = new List<HorarioDisponivel>();
        HorarioDisponivels = new List<HorarioDisponivel>();
        Agendamentos = new List<Agendamento>();
        TempoAtendimento = 25;
        TempoIntervalo = 5;
    }
    public Funcionario(string nome, string cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }

    public string Nome { get; set; }
    public string Cpf { get; set; }
    public Guid AdminId { get; set; }
    public int TempoAtendimento { get; set; } // Minutos
    public int TempoIntervalo { get; set; } // Minutos

    public List<HorarioDisponivel> HorarioDisponivels { get; set; }
    public List<Agendamento> Agendamentos { get; set; }
    public ConfiguracaoEmpresa? configuracaoEmpresa { get; set; }


    public List<HorarioDisponivel> GerarHorariosDisponiveis(List<HorarioFuncionamento> horariosFuncionamento)
    {
        if (TempoAtendimento <= 0)
            throw new InvalidOperationException("Duração do atendimento deve ser maior que zero");

        var horariosDisponiveis = new List<HorarioDisponivel>();

        foreach (var horarioFunc in horariosFuncionamento.Where(h => !h.Fechado))
        {
            if (!horarioFunc.HoraAbertura.HasValue || !horarioFunc.HoraFechamento.HasValue)
                continue;

            var horariosGerados = GerarHorariosDoDia(horarioFunc);
            horariosDisponiveis.AddRange(horariosGerados);
        }

        return horariosDisponiveis;
    }

    private List<HorarioDisponivel> GerarHorariosDoDia(HorarioFuncionamento horarioFuncionamento)
    {
        var horarios = new List<HorarioDisponivel>();
        var horarioAtual = horarioFuncionamento.HoraAbertura!.Value;
        var horarioFechamento = horarioFuncionamento.HoraFechamento!.Value;
        var duracaoTotal = TimeSpan.FromMinutes(TempoAtendimento + TempoIntervalo);

        while (horarioAtual.Add(TimeSpan.FromMinutes(TempoAtendimento)) <= horarioFechamento)
        {
            var horarioDisponivel = new HorarioDisponivel
            {
                Id = Guid.NewGuid(),
                FuncionarioId = Id,
                UsuarioId = AdminId, 
                DiaSemana = horarioFuncionamento.DiaSemana,
                HorarioInicio = horarioAtual,
                HorarioFim = horarioAtual.Add(TimeSpan.FromMinutes(TempoAtendimento)),
                Ativo = true
            };

            horarios.Add(horarioDisponivel);
            horarioAtual = horarioAtual.Add(duracaoTotal);
        }

        return horarios;
    }

    public void AtualizarTempoAtendimento(int duracaoMinutos, int intervaloMinutos)
    {
        if (duracaoMinutos <= 0)
            throw new ArgumentException("Duração deve ser maior que zero");

        if (intervaloMinutos < 0)
            throw new ArgumentException("Intervalo não pode ser negativo"); // corrigir

        TempoAtendimento = duracaoMinutos;
        TempoIntervalo = intervaloMinutos;
    }
}
