using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgenda;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloConfiguracao;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorarioFuncionamento;
using BarberClub.Dominio.ModuloServico;
using BarberClub.Infraestrutura.Orm.RepositorioConfiguracao;
using BarberClub.Infraestrutura.Orm.RepositorioFuncionario;
using BarberClub.Infraestrutura.Orm.RepositorioHorarioFuncionamento;
using BarberClub.Infraestrutura.Orm.RepositorioServicos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.Compartilhado;

public class BarberClubDbContext(DbContextOptions _options, ITenantProvider? _tenantProvider = null)
    : IdentityDbContext<Usuario, Cargo, Guid>(_options), IContextoPersistencia
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        if (_tenantProvider is not null)
        {
            builder.Entity<Funcionario>()
                .HasQueryFilter(f => (f.AdminId == _tenantProvider.EmpresaId
                && _tenantProvider.FuncionarioId == null)
                ||
                (f.AdminId == _tenantProvider.EmpresaId && f.Id == _tenantProvider.FuncionarioId)
                );

            builder.Entity<Servico>()
                .HasQueryFilter(s => (
                s.FuncionarioId == _tenantProvider.FuncionarioId)
                ||
                (_tenantProvider.FuncionarioId == null &&
                s.Funcionario!.AdminId == _tenantProvider.EmpresaId));

            builder.Entity<ConfiguracaoEmpresa>();

            builder.Entity<HorarioFuncionamento>()
                .HasQueryFilter(h => h.ConfiguracaoEmpresa!.UsuarioId == _tenantProvider.EmpresaId);

            builder.Entity<ConfiguracaoAgenda>()
                .HasQueryFilter(c => (c.FuncionarioId == _tenantProvider.FuncionarioId)
                ||
                (_tenantProvider.FuncionarioId == null &&
                 c.Funcionario!.AdminId == _tenantProvider.EmpresaId));
        }

    builder.ApplyConfiguration(new MapeadorFuncionarioEmOrm());
        builder.ApplyConfiguration(new MapeamentoServicoEmOrm());
        builder.ApplyConfiguration(new MapeamentoAgendaEmOrm());
        builder.ApplyConfiguration(new MapeamentoHorarioFuncionamentoEmOrm());
        builder.ApplyConfiguration(new MapeamentoAgendaEmOrm());

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
                    entry.State = EntityState.Unchanged;
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
        await Task.CompletedTask;
    }
}
