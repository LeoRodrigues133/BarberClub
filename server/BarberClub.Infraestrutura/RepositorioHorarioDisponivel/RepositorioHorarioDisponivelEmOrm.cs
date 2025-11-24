using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloHorario;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.RepositorioHorarioDisponivel;

public class RepositorioHorarioDisponivelEmOrm(IContextoPersistencia _context)
    :RepositorioBase<HorarioDisponivel>(_context), IRepositorioHorarioDisponivel

{

    public async Task<List<Guid>> CadastrarVariosAsync(List<HorarioDisponivel> horariosDisponiveis)
    {
        await _entities.AddRangeAsync(horariosDisponiveis);

        return horariosDisponiveis.Select(h => h.Id).ToList();
    }

    public async Task<HorarioDisponivel?> SelecionarPorIdAsync(Guid id)
    {
        return await _entities
            .Include(h => h.Agendamentos)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<List<HorarioDisponivel>> SelecionarTodosAsync()
    {
        return await _entities.ToListAsync();
    }
}