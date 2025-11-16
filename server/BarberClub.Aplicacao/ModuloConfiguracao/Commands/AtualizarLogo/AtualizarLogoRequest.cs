using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarLogo;

public record AtualizarLogoRequest(Guid empresaId, IFormFile arquivo) 
    : IRequest<Result<AtualizarLogoResponse>>;