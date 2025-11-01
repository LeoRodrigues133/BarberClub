using BarberClub.Dominio.ModuloAutenticacao;
using System.Security.Claims;

namespace BarberClub.WebApi.Identity;
public class ApiTenantProvider(IHttpContextAccessor _contextAccessor) : ITenantProvider
{
    public Guid? UsuarioId
    {
        get
        {
            var claimId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claimId is null)
                return null;

            return Guid.Parse(claimId.Value);
        }
    }

}