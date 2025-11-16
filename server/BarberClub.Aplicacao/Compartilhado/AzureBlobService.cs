using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Infraestrutura.Orm.Compartilhado;

namespace BarberClub.Aplicacao.Compartilhado;

public class AzureBlobService
    : IAzureBlobService
{
    readonly BlobServiceClient _blobServiceClient;
    readonly string _containerName;

    public AzureBlobService(IOptions<AzureBlobStorageConfig> settings)
    {
        var config = settings.Value;

        _blobServiceClient = new BlobServiceClient(config.ConnectionString);
        _containerName = config.ContainerName;
    }
    public record GerarTokenRequest(string Url);

    public record GerarTokenResponse(string UrlComToken);

    public async Task<string> UploadAsync(Guid empresaId, IFormFile arquivo, string pasta)
    {
        if (arquivo == null || arquivo.Length == 0)
            throw new ArgumentException("Arquivo inválido", nameof(arquivo));

        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png" };

        if (!extensoesPermitidas.Contains(extensao))
            throw new ArgumentException($"Apenas {string.Join(", ", extensoesPermitidas)} são permitidos");

        const long tamanhoMaximo = 5 * 1024 * 1024;
        if (arquivo.Length > tamanhoMaximo)
            throw new ArgumentException("Arquivo maior que 5MB");

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var prefixoBlob = $"{pasta}/empresaId--{empresaId}";

        await foreach (var blob in containerClient.GetBlobsAsync(prefix: prefixoBlob))
            await containerClient.DeleteBlobIfExistsAsync(blob.Name);

        var nomeBlob = $"{prefixoBlob}{extensao}";
        var blobClient = containerClient.GetBlobClient(nomeBlob);

        using var stream = arquivo.OpenReadStream();

        var contentType = extensao switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            _ => "application/octet-stream"
        };

        var headers = new BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(stream, headers);


        return blobClient.Uri.ToString();
    }

    public Task<bool> ExcluirAsync(string url)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GerarUrlToken(string url, TimeSpan expiracaoToken)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);

        var blobClient = blobContainer.GetBlobClient(url);

        if (!blobClient.CanGenerateSasUri)
            throw new InvalidOperationException("BlobClient deve ser autorizado com uma credencial de chave compartilhada para gerar um URI SAS.");

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = blobContainer.Name,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blobClient.GenerateSasUri(sasBuilder);

        return sasUri.ToString();
    }
}