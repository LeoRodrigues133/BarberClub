namespace BarberClub.Dominio.ModuloAgenda;
public interface IRepositorioAgenda
{
    Task<Guid> CadastrarAsync(ConfiguracaoAgenda newEntity);
    Task<ConfiguracaoAgenda?> SelecionarPorFuncionarioId(Guid funcionarioId);
    Task<bool> EditarAsync(ConfiguracaoAgenda agenda);
}
