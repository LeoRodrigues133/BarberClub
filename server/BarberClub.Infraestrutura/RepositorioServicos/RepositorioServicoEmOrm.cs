using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloServico;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.RepositorioServicos;

public class RepositorioServicoEmOrm(IContextoPersistencia _context)
    : RepositorioBase<Servico>(_context), IRepositorioServico
{
    public async Task<List<Servico>> SelecionarPorFuncionario(Guid funcionarioId)
    {
        return await _entities
                .Include(f => f.Funcionario)
                .Where(f => f.FuncionarioId == funcionarioId)
                .ToListAsync();

    }

    public async Task<Servico?> SelecionarPorIdAsync(Guid id)
    {
        return await _entities
            .Include(f => f.Funcionario)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Servico>> SelecionarTodosAsync()
    {
        return await _entities
            .Include(f => f.Funcionario)
            .ToListAsync();
    }

}