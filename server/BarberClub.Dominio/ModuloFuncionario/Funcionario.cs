using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgenda;

namespace BarberClub.Dominio.ModuloFuncionario;

public class Funcionario : EntidadeBase
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public Guid AdminId { get; set; }

    public Funcionario()
    { }
    public Funcionario(string nome, string cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }

}