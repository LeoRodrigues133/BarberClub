using BarberClub.Aplicacao.ModuloAutenticacao.DTOs;
using BarberClub.Aplicacao.ModuloFuncionario.DTOs;
using BarberClub.Dominio.ModuloAutenticacao;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.Cadastrar;

public record CadastrarFuncionarioRequest(
    string nome,
    string cpf,
    string userName,
    string password,
    string email,
    EnumCargo cargo
    ) : IRequest<Result<FuncionarioDto>>;