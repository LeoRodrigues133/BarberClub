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
            var cargo = roles.FirstOrDefault();

            funcionarios.Add(new SelecionarFuncionariosDto(
                registro.Id,
                registro.Nome,
                registro.Cpf,
                cargo!,
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