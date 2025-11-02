using BarberClub.Dominio.Compartilhado;

namespace BarberClub.Dominio.ModuloFuncionario;

public class Funcionario : EntidadeBase
{
    public string Nome {  get; set; }
    public string Cpf {  get; set; }
    public Guid adminId {  get; set; }

    public Funcionario(string nome, string cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }

}