namespace BarberClub.Dominio.Compartilhado;
public interface IContextoPersistencia
{
    Task<int> GravarAsync();
    Task DesfazerAsync();
}
