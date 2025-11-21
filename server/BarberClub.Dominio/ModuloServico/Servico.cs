using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;

namespace BarberClub.Dominio.ModuloServico;

public class Servico : EntidadeBase
{
    public Servico()
    { }

    public Servico(
        Guid funcionarioId,
        string titulo,
        decimal valor,
        int duracao,
        int? porcentagemPromocao = null,
        bool isPromocao = false
    )
    {
        FuncionarioId = funcionarioId;
        Titulo = titulo;
        Valor = valor;
        Duracao = duracao;
        IsPromocao = isPromocao;
        PorcentagemPromocao = porcentagemPromocao;
        Ativo = true;

        if (!porcentagemPromocao.HasValue)
            ValorFinal = CalcularValorPromocional(0);
        else
            ValorFinal = CalcularValorPromocional(porcentagemPromocao.Value);

    }

    public Guid FuncionarioId { get; set; }
    public string Titulo { get; set; }
    public decimal Valor { get; set; }
    public bool IsPromocao { get; set; }
    public int Duracao { get; set; }
    public int? PorcentagemPromocao { get; set; }
    public bool Ativo { get; set; }
    public Funcionario? Funcionario { get; set; }
    public decimal ValorFinal { get; set; }

    public decimal CalcularValorPromocional(int porcentagemPromocao)
    {
        if (this.IsPromocao == true)
            return this.ValorFinal = Valor * (1 - porcentagemPromocao / 100m);

        return ValorFinal = Valor;
    }

    public void AplicarPromocao(int porcentagemPromocao)
    {
        IsPromocao = true;
        PorcentagemPromocao = porcentagemPromocao;
        ValorFinal = Valor * (1 - porcentagemPromocao / 100m);
    }

    public void RemoverPromocao()
    {
        IsPromocao = false;
        PorcentagemPromocao = null;
        ValorFinal = Valor;
    }

    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;
}