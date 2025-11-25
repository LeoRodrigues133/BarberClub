using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloFuncionario;
using BarberClub.Dominio.ModuloHorario;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.DesativarHorario;

public class DesativarHorarioResquestHandler(
    IRepositorioHorarioDisponivel _repositorioHorario,
    IContextoPersistencia _context)
    : IRequestHandler<DesativarHorarioRequest, Result<DesativarHorarioResponse>>
{
    public async Task<Result<DesativarHorarioResponse>> Handle(
        DesativarHorarioRequest request, CancellationToken cancellationToken)
    {
        var horarioSelecionado = await _repositorioHorario
            .SelecionarPorIdAsync(request.horarioId);

        if (horarioSelecionado is null)
            return Result.Fail("Registro não encontrado");

        if (!horarioSelecionado.PodeDesativar(horarioSelecionado.DataEspecifica))
            return Result.Fail("Não é possível fechar o horário selecionado");

        try
        {
            horarioSelecionado.Desativar();

            await _repositorioHorario.EditarAsync(horarioSelecionado);
            await _context.GravarAsync();

        }
        catch (Exception ex)
        {

            await _context.DesfazerAsync();
            return Result.Fail(ex.ToString());
        }

        return Result.Ok(new DesativarHorarioResponse(horarioSelecionado.Id));
    }
}