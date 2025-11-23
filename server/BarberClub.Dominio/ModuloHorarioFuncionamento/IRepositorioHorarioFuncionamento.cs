using BarberClub.Dominio.ModuloFuncionario;

namespace BarberClub.Dominio.ModuloHorarioFuncionamento;
public interface IRepositorioHorarioFuncionamento
{
    Task<bool> EditarAsync(HorarioFuncionamento editEntity);
    Task<List<HorarioFuncionamento>> SelecionarTodosAsync();
    Task<HorarioFuncionamento> SelecionarPorIdAsync(Guid id);

}
