using BarberClub.Dominio.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.Compartilhado;

public class RepositorioBase<TEntity> where TEntity : EntidadeBase
{
    protected readonly IContextoPersistencia _contextoPersistencia;
    protected readonly DbSet<TEntity> _entities;

    public RepositorioBase(
        IContextoPersistencia contextoPersistencia
        )
    {
        _contextoPersistencia = contextoPersistencia;
        _entities = ((DbContext)this._contextoPersistencia).Set<TEntity>();
    }

    public async Task<Guid> CadastrarAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);

        return entity.Id;
    }

    public async Task<bool> EditarAsync(TEntity entity)
    {
        var finder = _entities.Update(entity);

        return await Task.Run(() => finder.State == EntityState.Modified);
    }

    public async Task<bool> ExcluirAsync(TEntity entity)
    {
        var finder = _entities.Remove(entity);

        return await Task.Run(() => finder.State == EntityState.Deleted);
    }

    public async Task<List<TEntity>> SelecionarTodosAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<TEntity?> SelecionarPorIdAsync(Guid id)
    {
        return await _entities.SingleOrDefaultAsync(e => e.Id == id);
    }
}
