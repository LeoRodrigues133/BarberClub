using BarberClub.Aplicacao.ModuloFuncionario.DTOs;

namespace BarberClub.Aplicacao.ModuloHorarioDisponivel.Commands.CadastrarVariosHorarios;

public record CadastrarVariosHorariosResponse(
    int qtHorariosGerados,
    int qtHorariosRemovidos,
    List<HorarioCadastradoDto> HorariosCadastrados);