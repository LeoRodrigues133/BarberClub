using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;

namespace BarberClub.Dominio.ModuloServico;

public class Servico : EntidadeBase
{
    public Servico()
    { }

    public Servico(
        string titulo,
        decimal valor,
        int? duracao // Em minutos
        )
    {
        Titulo = titulo;
        Valor = valor;
        Duracao = duracao;
        IsPromocao = false;
        PorcentagemPromocao = null;
    }
    public Servico(
        Guid funcionarioId,
        string titulo,
        decimal valor,
        int? duracao,
        int? porcentagemPromocao = null,
        bool? isPromocao = null
    )
    {
        FuncionarioId = funcionarioId;

        Titulo = titulo;
        Valor = valor;
        Duracao = duracao;
        IsPromocao = isPromocao ?? false;
        PorcentagemPromocao = porcentagemPromocao;
        Ativo = true;

        if (IsPromocao && PorcentagemPromocao.HasValue)
        {
            ValorFinal = Valor * (1 - PorcentagemPromocao.Value / 100m);
        }
        else
        {
            ValorFinal = Valor;
        }
    }

    public Guid FuncionarioId { get; set; }
    public string Titulo { get; set; }
    public decimal Valor { get; set; }
    public bool IsPromocao { get; set; }
    public int? Duracao { get; set; }
    public int? PorcentagemPromocao { get; set; }
    public bool Ativo { get; set; }

    public Funcionario? Funcionario { get; set; }

    public decimal ValorFinal { get; set; }

    public decimal CalcularValorPromocional(int porcentagemPromocao)
    {
        if (this.IsPromocao is true)
            return this.ValorFinal = ValorFinal * Math.Floor(1 - PorcentagemPromocao.Value / 100m);

        return ValorFinal = Valor;
    }
    public void AplicarPromocao(int porcetagemPromocao)
    {
        IsPromocao = true;
        PorcentagemPromocao = porcetagemPromocao;
    }

    public void RemoverPromocao()
    {
        IsPromocao = false;
        PorcentagemPromocao = null;
    }

    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;
}