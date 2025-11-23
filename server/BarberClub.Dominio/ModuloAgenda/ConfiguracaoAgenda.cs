using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;

namespace BarberClub.Dominio.ModuloAgenda;

public class ConfiguracaoAgenda : EntidadeBase
{

    public ConfiguracaoAgenda()
    { }
    public ConfiguracaoAgenda(
        Guid funcionarioId,
        int tempoAtendimento,
        int intervaloEntreAtendimentos)
    {
        FuncionarioId = funcionarioId;
        TempoAtendimento = tempoAtendimento;
        IntervaloEntreAtendimento = intervaloEntreAtendimentos;
    }

    public Guid FuncionarioId { get; set; }
    public int TempoAtendimento { get; set; } // Minutos
    public int IntervaloEntreAtendimento { get; set; } // Minutos

    public Funcionario Funcionario { get; set; }

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
        var duracaoTotal = TimeSpan.FromMinutes(TempoAtendimento + IntervaloEntreAtendimento);

        while (horarioAtual.Add(TimeSpan.FromMinutes(TempoAtendimento)) <= horarioFechamento)
        {
            var horarioDisponivel = new HorarioDisponivel
            {
                Id = Guid.NewGuid(),
                FuncionarioId = FuncionarioId,
                HorarioFuncionamentoId = horarioFuncionamento.Id,
                HorarioInicio = horarioAtual,
                HorarioFim = horarioAtual.Add(TimeSpan.FromMinutes(TempoAtendimento)),
                Ativo = true
            };

            horarios.Add(horarioDisponivel);
            horarioAtual = horarioAtual.Add(duracaoTotal);
        }

        return horarios;
    }

    private ConfiguracaoAgenda CriarConfiguracaoInicial(Guid FuncionarioId)
    {
        var tempoAtendimentoPadrao = 25;
        var intervaloAtendimentoPadrao = 5;

        var configuracaoInicial = new ConfiguracaoAgenda(
            FuncionarioId,
            tempoAtendimentoPadrao,
            intervaloAtendimentoPadrao);

        return configuracaoInicial;
    }

}