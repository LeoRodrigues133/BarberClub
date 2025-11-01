using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Dominio.Compartilhado;

public class EntidadeBase
{
    public Guid Id { get; set; }

    protected EntidadeBase()
    {
        Id = Guid.NewGuid();
    }


    public Guid UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }
}


