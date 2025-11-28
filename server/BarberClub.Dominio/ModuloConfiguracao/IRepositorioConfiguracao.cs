namespace BarberClub.Dominio.ModuloConfiguracao;
public interface IRepositorioConfiguracao
{
    Task<ConfiguracaoEmpresa?> SelecionarPorEmpresaIdComHorariosAsync(Guid usuarioAdminId);
    Task<ConfiguracaoEmpresa?> SelecionarPorIdAsync(Guid Id);
    Task<bool> ExisteConfiguracaoAsync(Guid usuarioAdminId);
    Task<Guid> CadastrarConfiguracaoInicialAsync(ConfiguracaoEmpresa configuracaoEmpresa);
    Task<bool> EditarAsync(ConfiguracaoEmpresa configuracaoEmpresa);
}
