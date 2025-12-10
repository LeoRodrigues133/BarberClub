using BarberClub.Dominio.ModuloAutenticacao;

namespace BarberClub.Aplicacao.ModuloFuncionario.DTOs;

public record EditarFuncionarioDto(string? nome, string? nomeApresentacao, string? cpf, string? email, EnumCargo? cargo);