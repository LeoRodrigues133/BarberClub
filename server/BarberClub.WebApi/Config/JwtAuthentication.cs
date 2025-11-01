using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BarberClub.WebApi.Config;
public static class JwtAuthentication
{
    public static void JwtAuthenticationConfiguration(
        this IServiceCollection _service,
        IConfiguration _configuration)
    {
        var chaveJwt = _configuration["JWT_GENERATION_KEY"];

        if (chaveJwt is null)
            throw new ArgumentNullException("Não foi possível obter assinatura de tokens.");

        var chaveJwtEmBytes = Encoding.ASCII.GetBytes(chaveJwt);

        var audienciaValida = _configuration["JWT_AUDIENCE_DOMAIN"];

        if (audienciaValida is null)
            throw new ArgumentNullException("Não foi possível obter o domínio da audiência dos tokens.");

        _service.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;

            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(chaveJwtEmBytes),
                ValidAudience = audienciaValida,
                ValidIssuer = "BarberClub",
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true
            };
        });

        _service.AddAuthorization();
    }
}