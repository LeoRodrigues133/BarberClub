using BarberClub.Aplicacao.ModuloConfiguracao.Services;
namespace BarberClub.WebApi.Config;
public static class ApplicationServicesDependencyInjection
{
    public static void ApplicationServicesConfiguration(this IServiceCollection _service)
    {
        _service.AddScoped<ServiceConfiguracao>();

    }
}