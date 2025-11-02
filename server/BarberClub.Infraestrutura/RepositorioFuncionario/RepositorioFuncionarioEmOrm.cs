using BarberClub.Dominio.Compartilhado;
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
}