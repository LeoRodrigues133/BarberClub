using BarberClub.Aplicacao.ModuloConfiguracao.DTOs;
using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.AtualizarConfiguracao;

public record AtualizarConfiguracaoRequest(
    Guid adminId,
    string nomeEmpresa,
    List<HorarioFuncionamentoDto> horarios)
    : IRequest<Result<AtualizarConfiguracaoResponse>>;