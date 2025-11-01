namespace BarberClub.Dominio.ModuloAutenticacao;
public interface ITenantProvider
{
    Guid? UsuarioId { get; }
}
