using BarberClub.Dominio.Compartilhado;
using Microsoft.EntityFrameworkCore;
using BarberClub.Dominio.ModuloConfiguracao.ModuloHorarioFuncionamento;

namespace BarberClub.Infraestrutura.Orm.RepositorioConfiguracao.RepositorioHorarioFuncionamento;

public class RepositorioHorarioFuncionamentoEmOrm
    : IRepositorioHorarioFuncionamento
{
    readonly DbSet<HorarioFuncionamento> horarios;

    public RepositorioHorarioFuncionamentoEmOrm(IContextoPersistencia _context)
    {
        horarios = ((DbContext)_context).Set<HorarioFuncionamento>();
    }

    public async Task<bool> EditarAsync(HorarioFuncionamento editEntity)
    {
        var finder = horarios.Update(editEntity);

        return await Task.Run(() => finder.State == EntityState.Modified);
    }

    public async Task<HorarioFuncionamento?> SelecionarPorIdAsync(Guid id)
    {
        return await horarios.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<HorarioFuncionamento>> SelecionarTodosAsync()
    {
        return await horarios.ToListAsync();
    }

}