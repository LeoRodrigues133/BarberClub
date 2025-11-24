using BarberClub.Aplicacao.ModuloFuncionario.DTOs;

namespace BarberClub.Aplicacao.ModuloFuncionario.Commands.CadastrarVariosHorarios;

public record CadastrarVariosHorariosResponse(
    int qtHorariosGerados,
    List<HorarioCadastradoDto> HorariosCadastrados);