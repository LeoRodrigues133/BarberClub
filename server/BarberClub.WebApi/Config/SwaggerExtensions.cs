using Microsoft.OpenApi.Models;

namespace BarberClub.WebApi.Config;
public static class SwaggerExtensions
{
    public static void SwaggerConfiguration(this IServiceCollection _service)
    {
        _service.AddEndpointsApiExplorer();
        
        _service.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo { Title = "BarberClub.WebApi", Version = "v1" });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Informe o token no padrão {Bearer}.",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id= "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }
}
