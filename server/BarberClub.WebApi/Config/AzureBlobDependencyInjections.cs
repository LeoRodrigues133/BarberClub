using BarberClub.Aplicacao.Compartilhado;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Infraestrutura.Orm.Compartilhado;

namespace BarberClub.WebApi.Config;
public static class AzureBlobDependencyInjections
{
    public static void AzureServicesDependenciesConfiguration(this IServiceCollection _service, IConfiguration _configuration)
    {
        _service.AddScoped<IAzureBlobService, AzureBlobService>();

        _service.Configure<AzureBlobStorageConfig>(_configuration.GetSection("AZURE_BLOB_STORAGE"));
    }
}