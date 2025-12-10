using BarberClub.Aplicacao.ModuloAutenticacao.DTOs;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloAutenticacao.Commands.Registrar;

public record RegistrarUsuarioRequest(string userName, string password, string email, string nomeApresentacao, string nomeEmpresa)
   : IRequest<Result<TokenResponse>>;