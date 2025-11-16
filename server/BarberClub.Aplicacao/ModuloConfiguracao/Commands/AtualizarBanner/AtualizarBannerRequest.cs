using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarBanner;

public record AtualizarBannerRequest(Guid empresaId,IFormFile arquivo)
    : IRequest<Result<AtualizarBannerResponse>>;