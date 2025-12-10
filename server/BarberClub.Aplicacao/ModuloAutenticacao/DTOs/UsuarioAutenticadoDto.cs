using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloAutenticacao.DTOs;

public class UsuarioAutenticadoDto
{
    public required Guid Id { get; set; }
    public required Guid? FuncionarioId { get; set; }
    public required Guid EmpresaId { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string NomeApresentacao { get; set; }
    public required EnumCargo Role { get; set; }
}