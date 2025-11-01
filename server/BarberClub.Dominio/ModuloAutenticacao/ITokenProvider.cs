namespace BarberClub.Dominio.ModuloAutenticacao;
public interface ITokenProvider
{
    IAccessToken GerarAccessToken(Usuario usuario);
}
