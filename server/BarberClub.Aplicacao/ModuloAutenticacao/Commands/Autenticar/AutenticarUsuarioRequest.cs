using BarberClub.Aplicacao.ModuloAutenticacao.DTOs;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloAutenticacao.Commands.Autenticar;

public record AutenticarUsuarioRequest(string userName, string password)
    : IRequest<Result<TokenResponse>>;