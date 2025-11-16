using Microsoft.AspNetCore.Http;

namespace BarberClub.Aplicacao.ModuloConfiguracao.DTOs;

public record AtualizarArquivoDto(IFormFile arquivo);