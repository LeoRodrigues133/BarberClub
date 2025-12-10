using BarberClub.Aplicacao.ModuloConfiguracao.DTOs;

namespace BarberClub.Aplicacao.ModuloConfiguracao.Commands.CarregarConfiguracao;

public record CarregarConfiguracaoResponse(
    Guid Id,
    string NomeEmpresa,
    string LogoUrl,
    string BannerUrl,
    bool Ativo,
    DateTime DataCriacao,
    List<HorarioFuncionamentoDto> HorarioDeExpediente,
    List<DateTime> DatasEspecificasFechado
);