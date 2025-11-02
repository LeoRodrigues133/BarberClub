namespace BarberClub.Dominio.ModuloFuncionario;
public interface IRepositorioFuncionario
{
    Task<Guid> CadastrarAsync(Funcionario newEntity);
    Task<bool> EditarAsync(Funcionario editEntity);
    Task<bool> ExcluirAsync(Funcionario deleteEntity);
    Task<List<Funcionario>> SelecionarTodosAsync();
    Task<Funcionario> SelecionarPorIdAsync(Guid id);
    Task<bool> ExistePorCpfAsync(string cpf);

}
