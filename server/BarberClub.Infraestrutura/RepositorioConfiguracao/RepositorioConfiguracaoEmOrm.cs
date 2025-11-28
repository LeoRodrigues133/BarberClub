using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloConfiguracao;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.RepositorioConfiguracao;

public class RepositorioConfiguracaoEmOrm(IContextoPersistencia _context)
    : RepositorioBase<ConfiguracaoEmpresa>(_context), IRepositorioConfiguracao
{
    public async Task<Guid> CadastrarConfiguracaoInicialAsync(ConfiguracaoEmpresa configuracaoEmpresa)
    {
        await _entities.AddAsync(configuracaoEmpresa);

        return configuracaoEmpresa.Id;
    }

    public async Task<ConfiguracaoEmpresa> SelecionarPorIdAsync(Guid id)
    {
        return await _entities
            .Include(x =>x.HorarioDeExpediente)
            .Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<bool> ExisteConfiguracaoAsync(Guid usuarioAdminId)
    {
        return await _entities.AnyAsync(x => x.UsuarioId == usuarioAdminId);
    }

    public async Task<ConfiguracaoEmpresa?> SelecionarPorEmpresaIdComHorariosAsync(Guid usuarioAdminId)
    {
        return await _entities.Where(x => x.UsuarioId == usuarioAdminId)
    .Include(x => x.HorarioDeExpediente).FirstOrDefaultAsync();
    }
}