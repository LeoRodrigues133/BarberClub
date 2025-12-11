namespace BarberClub.Dominio.ModuloHorario;
public interface IRepositorioHorarioDisponivel
{
    Task<List<HorarioDisponivel>> SelecionarTodosAsync();
    Task<HorarioDisponivel?> SelecionarPorIdAsync(Guid id);
    Task<List<HorarioDisponivel?>> SelecionarPorFuncionarioEPeriodoAsync(Guid id, int mes, int ano);
    Task<List<Guid>> CadastrarVariosAsync(List<HorarioDisponivel> horariosDisponiveis);
    Task<bool> EditarAsync(HorarioDisponivel horarioDisponivel);
    Task<bool> ExcluirAsync(Guid id);
}