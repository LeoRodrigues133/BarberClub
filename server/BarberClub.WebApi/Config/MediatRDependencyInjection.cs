using BarberClub.Aplicacao.ModuloAutenticacao.Commands.Autenticar;

namespace BarberClub.WebApi.Config;
public static class MediatRDependencyInjection
{
    public static void MediatRConfiguration(this IServiceCollection _service)
    {
        _service.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AutenticarUsuarioRequest>();
        });
    }
}