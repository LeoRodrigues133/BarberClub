using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloAutenticacao.DTOs;

public record TokenResponse : IAccessToken
{
    public required string Chave {  get; set; }
    public required DateTime expiracaoToken { get; set; }
    public required UsuarioAutenticadoDto Usuario { get; set; }
}