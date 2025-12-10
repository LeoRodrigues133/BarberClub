using BarberClub.Aplicacao.ModuloFuncionario.DTOs;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloFuncionario;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;

public class SelecionarFuncionariosRequestHandler(
    IRepositorioFuncionario _repositorioFuncionario,
    UserManager<Usuario> _userManager
    )
    : IRequestHandler<SelecionarFuncionariosRequest, Result<SelecionarFuncionariosResponse>>
{
    public async Task<Result<SelecionarFuncionariosResponse>> Handle(SelecionarFuncionariosRequest request, CancellationToken cancellationToken)
    {
        var registros = await _repositorioFuncionario.SelecionarTodosAsync();
        var funcionarios = new List<SelecionarFuncionariosDto>();

        foreach (var registro in registros)
        {
            var usuario = await _userManager.FindByIdAsync(registro.UsuarioId.ToString());

            var roles = await _userManager.GetRolesAsync(usuario);
            var cargoString = roles.FirstOrDefault();

            if (cargoString is null)
                return Result.Fail("Cargo não encontrado para o registro");

            if (!Enum.TryParse<EnumCargo>(cargoString, true, out var cargoEnum))
                return Result.Fail($"Cargo '{cargoString}' não é válido");

            funcionarios.Add(new SelecionarFuncionariosDto(
                registro.Id,
                registro.Nome,
                registro.Usuario!.NomeApresentacao,
                registro.Cpf,
                cargoEnum,
                registro.Usuario!.Email!,
                registro.TempoAtendimento,
                registro.TempoIntervalo
            ));
        }

        var resposta = new SelecionarFuncionariosResponse
        {
            Funcionarios = funcionarios
        };

        return Result.Ok(resposta);
    }
}