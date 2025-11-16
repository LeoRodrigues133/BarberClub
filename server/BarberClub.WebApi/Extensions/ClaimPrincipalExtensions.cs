using System.Security.Claims;

namespace BarberClub.WebApi.Extensions;
public static class ClaimPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("Usuário não autenticado");

        if(!Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidOperationException("UserId inválido no token");

        return userId;
    }

    public static Guid GetEmpresaId(this ClaimsPrincipal empresaUser)
    {
        var empresaIdClaim = empresaUser.FindFirst("EmpresaId")?.Value;

        if (!Guid.TryParse(empresaIdClaim, out var empresaId))
            throw new InvalidOperationException("EmpresaId inválido no token");

        return empresaId;

    }
}