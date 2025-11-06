using BarberClub.Dominio.ModuloAutenticacao;
using System.Security.Claims;

namespace BarberClub.WebApi.Identity;
public class ApiTenantProvider(IHttpContextAccessor _contextAccessor) : ITenantProvider
{
    public Guid? EmpresaId
    {
        get
        {
            var empresaId = _contextAccessor.HttpContext?.User.FindFirst("EmpresaId")?.Value;
            Console.WriteLine($"Claim EmpresaId: {empresaId}");

            if (empresaId is null)
                return null;

            return Guid.Parse(empresaId);
        }
    }


    public Guid? FuncionarioId
    {
        get
        {
            var funcionarioId = _contextAccessor.HttpContext?.User.FindFirst("FuncionarioId")?.Value;
            Console.WriteLine($"Claim FuncionarioId: {funcionarioId}");

            if (funcionarioId is null)
                return null;

            return Guid.Parse(funcionarioId);
        }
    }
}