using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloFuncionario.DTOs;

public record EditarFuncionarioDto(string? nome, string? cpf, EnumCargo? cargo);