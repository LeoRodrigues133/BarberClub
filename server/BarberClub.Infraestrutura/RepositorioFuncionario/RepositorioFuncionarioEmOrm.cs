using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.RepositorioFuncionario;

public class RepositorioFuncionarioEmOrm(IContextoPersistencia _context) 
    : RepositorioBase<Funcionario>(_context), IRepositorioFuncionario
{
    public Task<Funcionario?> SelecionarPorIdAsync(Guid id)
    {
        return _entities
            .Include(f => f.Usuario)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Funcionario>> SelecionarTodosAsync()
    {
        return await _entities
            .Include(f => f.Usuario)
            .ToListAsync();
    }

    public async Task<bool> ExistePorCpfAsync(string cpf)
    {
        return await _entities.AnyAsync(x => x.Cpf == cpf);
    }


    public async Task<Funcionario?> SelecionarPorUsuarioAsync(Guid usuarioId)
    {
        return await _entities
            .Include(f => f.Usuario)
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId);
    }
    public async Task<Funcionario?> SelecionarTodosSemFiltroAsync(Guid usuarioId)
    {
        return await _entities
            .IgnoreQueryFilters()
            .Include(f => f.Usuario)
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId);
    }
}