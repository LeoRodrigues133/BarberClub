using BarberClub.Dominio.Compartilhado;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.WebApi.Config;
public static class DataDependencyInjections
{
    public static void DbContextConfiguration(this IServiceCollection _services, IWebHostEnvironment _environment, IConfiguration _configuration
        )
    {
        var connectionString = _configuration["SQLSERVER_CONNECTION_STRING"];

        if (connectionString is null)
            throw new ArgumentNullException("\"SQLSERVER_CONNECTION_STRING\" não foi fornecida para o ambiente");
        _services.AddDbContext<IContextoPersistencia, BarberClubDbContext>(options =>
        {
            if (!_environment.IsDevelopment())
                options.EnableSensitiveDataLogging(false);

            options.UseSqlServer(connectionString, dbOptions =>
            {
                dbOptions.EnableRetryOnFailure();
            });
        });

    }
}