using Microsoft.AspNetCore.Identity;

namespace BarberClub.Dominio.ModuloAutenticacao;

public class Cargo : IdentityRole<Guid>
{

    public Cargo()
        : base()
    {
        Id = Guid.NewGuid();
    }

    public Cargo(EnumCargo cargo)
        : base(cargo.ToString())
    {
        Id = Guid.NewGuid();
    }
}