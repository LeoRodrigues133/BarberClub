using BarberClub.Aplicacao.ModuloAutenticacao.Services;
using BarberClub.WebApi.Identity;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.AspNetCore.Identity;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Infraestrutura.Orm.RepositorioFuncionario;

namespace BarberClub.WebApi.Config;
public static class AuthDependencyInjections
{
    public static void AuthProviderConfiguration(this IServiceCollection _service)
    {
        _service.AddScoped<ITokenProvider, JwtProvider>();
        _service.AddScoped<ITenantProvider, ApiTenantProvider>();

        _service.AddIdentity<Usuario, Cargo>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<BarberClubDbContext>()
            .AddDefaultTokenProviders();
    }
}