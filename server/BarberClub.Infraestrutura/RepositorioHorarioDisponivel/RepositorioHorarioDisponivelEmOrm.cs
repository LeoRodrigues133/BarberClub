using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloHorario;
using BarberClub.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Infraestrutura.Orm.RepositorioHorarioDisponivel;

public class RepositorioHorarioDisponivelEmOrm(IContextoPersistencia _context)
    : RepositorioBase<HorarioDisponivel>(_context), IRepositorioHorarioDisponivel

{

    public async Task<List<Guid>> CadastrarVariosAsync(List<HorarioDisponivel> horariosDisponiveis)
    {
        await _entities.AddRangeAsync(horariosDisponiveis);

        return horariosDisponiveis.Select(h => h.Id).ToList();
    }

    public async Task<List<HorarioDisponivel>> SelecionarPorFuncionarioEPeriodoAsync(Guid id, int mes, int ano)
    {
        return await _entities
            .Include(h => h.Agendamentos)
            .Where(h => h.FuncionarioId == id
                    && h.DataEspecifica.Month == mes
                    && h.DataEspecifica.Year == ano)
            .ToListAsync();

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

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var horario = await _entities
            .FirstOrDefaultAsync(h => h.Id == id);

        if (horario is null)
            return false;

        _entities.Remove(horario);
        
        return true;
    }
}