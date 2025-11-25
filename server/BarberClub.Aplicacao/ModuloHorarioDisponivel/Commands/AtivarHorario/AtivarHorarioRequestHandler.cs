using BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.DesativarHorario;
using BarberClub.Dominio.Compartilhado;
using BarberClub.Dominio.ModuloHorario;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.AtivarHorario;

public class AtivarHorarioRequestHandler(
    IRepositorioHorarioDisponivel _repositorioHorario,
    IContextoPersistencia _context)
    : IRequestHandler<AtivarHorarioRequest, Result<AtivarHorarioResponse>>
{
    public async Task<Result<AtivarHorarioResponse>> Handle(AtivarHorarioRequest request, CancellationToken cancellationToken)
    {
        var horarioSelecionado = await _repositorioHorario.SelecionarPorIdAsync(request.horarioId);

        if (horarioSelecionado is null)
            return Result.Fail("Registro não encontrado");

        if(!horarioSelecionado.PodeAtivar(horarioSelecionado.DataEspecifica))
            return Result.Fail("Não é possível abrir o horário selecionado");

        try
        {
            horarioSelecionado.Ativar();

            await _repositorioHorario.EditarAsync(horarioSelecionado);
            await _context.GravarAsync();

        }
        catch (Exception ex)
        {

            await _context.DesfazerAsync();
            return Result.Fail(ex.ToString());
        }

        return Result.Ok(new AtivarHorarioResponse(horarioSelecionado.Id));
    }
}