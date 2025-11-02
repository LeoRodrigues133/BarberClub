using BarberClub.Aplicacao.ModuloAutenticacao.DTOs;
using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloFuncionario.DTOs;

public class FuncionarioDto : UsuarioAutenticadoDto
{
    public required EnumCargo Cargo {  get; set; }
}