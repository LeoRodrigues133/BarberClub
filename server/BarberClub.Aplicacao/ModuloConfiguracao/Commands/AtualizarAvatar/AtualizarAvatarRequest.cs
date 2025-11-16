using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarAvatar;

public record AtualizarAvatarRequest(Guid funcionarioId, IFormFile arquivo)
    : IRequest<Result<AtualizarAvatarResponse>>;