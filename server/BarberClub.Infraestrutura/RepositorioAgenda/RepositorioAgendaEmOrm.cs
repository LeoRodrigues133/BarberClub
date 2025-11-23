using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloAgenda;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.RepositorioAgenda;

public class RepositorioAgendaEmOrm(IContextoPersistencia _context)
    : RepositorioBase<ConfiguracaoAgenda>(_context), IRepositorioAgenda
{
    public async Task<ConfiguracaoAgenda?> SelecionarPorFuncionarioId(Guid funcionarioId)
    {
        return await _entities.Where(x => x.FuncionarioId == funcionarioId).SingleOrDefaultAsync();
    }
}