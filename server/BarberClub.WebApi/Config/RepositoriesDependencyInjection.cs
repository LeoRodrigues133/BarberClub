using BarberClub.Dominio.ModuloAgenda;
using BarberClub.Dominio.ModuloConfiguracao;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;
using BarberClub.Dominio.ModuloServico;
using BarberClub.Infraestrutura.Orm.RepositorioAgenda;
using BarberClub.Infraestrutura.Orm.RepositorioConfiguracao;
using BarberClub.Infraestrutura.Orm.RepositorioFuncionario;
using BarberClub.Infraestrutura.Orm.RepositorioHorarioFuncionamento;
using BarberClub.Infraestrutura.Orm.RepositorioServicos;

namespace BarberClub.WebApi.Config;
public static class RepositoriesDependencyInjection
{
    public static void RepositoriesConfiguration(this IServiceCollection _service)
    {
        _service.AddScoped<IRepositorioFuncionario, RepositorioFuncionarioEmOrm>();
        _service.AddScoped<IRepositorioServico, RepositorioServicoEmOrm>();
        _service.AddScoped<IRepositorioConfiguracao, RepositorioConfiguracaoEmOrm>();
        _service.AddScoped<IRepositorioHorarioFuncionamento, RepositorioHorarioFuncionamentoEmOrm>();
        _service.AddScoped<IRepositorioAgenda, RepositorioAgendaEmOrm>();
    }
}