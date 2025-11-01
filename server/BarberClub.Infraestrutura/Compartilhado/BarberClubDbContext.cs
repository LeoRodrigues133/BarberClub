using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAutenticacao;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BarberClub.Infraestrutura.Orm.Compartilhado;

public class BarberClubDbContext(DbContextOptions _options, ITenantProvider? _tenantProvider = null)
    : IdentityDbContext<Usuario, Cargo, Guid>(_options), IContextoPersistencia
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        if (_tenantProvider is not null)
        {
            //builder.Entity<Funcionario>().HasQueryFilter(f => f.UsuarioId == _tenantProvider.UsuarioId);
        }

        //modelBuilder.ApplyConfiguration(new MapeadorFuncionarioEmOrm());

        base.OnModelCreating(builder);
    }
    public async Task<int> GravarAsync()
    {
        return await SaveChangesAsync();
    }

    public async Task DesfazerAsync()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Deleted;
                    break;
            }
        }
        await Task.CompletedTask;
    }
}
