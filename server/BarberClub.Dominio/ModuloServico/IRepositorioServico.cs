namespace BarberClub.Dominio.ModuloServico;
public interface IRepositorioServico
{
    Task<Guid> CadastrarAsync(Servico newEntity);
    Task<bool> EditarAsync(Servico editEntity);
    Task<bool> ExcluirAsync(Servico deleteEntity);
    Task<List<Servico>> SelecionarTodosAsync();
    Task<List<Servico>> SelecionarPorFuncionario(Guid funcionarioId);
    Task<Servico> SelecionarPorIdAsync(Guid id);
}
