namespace BarberClub.Dominio.ModuloAutenticacao;
public interface ITenantProvider
{
    Guid? EmpresaId { get; }
    Guid? FuncionarioId { get;}

}
