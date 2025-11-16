namespace BarberClub.Infraestrutura.Orm.Compartilhado;

public record AzureBlobStorageConfig
{
    public string ConnectionString { get; init; }
    public string ContainerName { get; init; }
}