using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.WebApi.Controllers;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;

public class SelecionarFuncionarioPorIdRequestHandler(
    IRepositorioFuncionario _repositorioFuncionario,
    UserManager<Usuario> _userManager)
    : IRequestHandler<SelecionarFuncionarioPorIdRequest, Result<SelecionarFuncionarioPorIdResponse>>
{
    public async Task<Result<SelecionarFuncionarioPorIdResponse>> Handle(
        SelecionarFuncionarioPorIdRequest request, CancellationToken cancellationToken)
    {

        var funcionarioSelecionado = await _repositorioFuncionario.SelecionarPorIdAsync(request.id);

        if (funcionarioSelecionado is null)
            return Result.Fail("Registro não escontrado");

        var roles = await _userManager.GetRolesAsync(funcionarioSelecionado.Usuario!);
        var cargoString = roles.FirstOrDefault();

        if (cargoString is null)
            return Result.Fail("Cargo não encontrado para o registro");

        if (!Enum.TryParse<EnumCargo>(cargoString, true, out var cargoEnum))
            return Result.Fail($"Cargo '{cargoString}' não é válido");

        var resposta = new SelecionarFuncionarioPorIdResponse(
            funcionarioSelecionado.Id,
            funcionarioSelecionado.Nome,
            funcionarioSelecionado.Cpf,
            cargoEnum,
            funcionarioSelecionado.Usuario!.Email!,
            funcionarioSelecionado.TempoAtendimento,
            funcionarioSelecionado.TempoIntervalo
            );

        return Result.Ok(resposta);
    }
}