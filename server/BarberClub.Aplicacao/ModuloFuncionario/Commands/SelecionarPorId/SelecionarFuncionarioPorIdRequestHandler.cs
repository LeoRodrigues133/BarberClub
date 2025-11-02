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
        var cargo = roles.FirstOrDefault();

        var resposta = new SelecionarFuncionarioPorIdResponse(
            funcionarioSelecionado.Id,
            funcionarioSelecionado.Nome,
            funcionarioSelecionado.Cpf,
            cargo
            );

        return Result.Ok(resposta);
    }
}