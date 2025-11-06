using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloServico;
using BarberClub.Infraestrutura.Orm.RepositorioFuncionario;
using BarberClub.Infraestrutura.Orm.RepositorioServicos;

namespace BarberClub.WebApi.Config;
public static class RepositoriesDependencyInjection
{
    public static void RepositoriesConfiguration(this IServiceCollection _service)
    {
        _service.AddScoped<IRepositorioFuncionario, RepositorioFuncionarioEmOrm>();
        _service.AddScoped<IRepositorioServico, RepositorioServicoEmOrm>();
    }
}