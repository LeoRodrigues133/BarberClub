using BarberClub.Infraestrutura.Orm.Compartilhado;

namespace BarberClub.WebApi.Config;
public static class DataMigrator
{
    public static bool AutoMigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BarberClubDbContext>();

        var migrations = MigradorBancoOrm.AtualizarBancoDados(dbContext);

        return migrations;
    }
}