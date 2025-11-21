using Microsoft.AspNetCore.Http;

namespace BarberClub.Dominio.Compartilhado;
public interface IAzureBlobService
{
    Task<string> UploadAsync(Guid adminId,IFormFile arquivo, string pasta);
    Task<bool> ExcluirAsync(string url);
    Task<string> GerarUrlComToken(string nomeBlob, TimeSpan expiracaoToken);
}
