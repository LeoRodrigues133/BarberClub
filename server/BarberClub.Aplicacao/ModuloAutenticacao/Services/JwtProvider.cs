using BarberClub.Aplicacao.ModuloAutenticacao.DTOs;
using BarberClub.Dominio.ModuloAutenticacao;
using BarberClub.Dominio.ModuloFuncionario;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberClub.Aplicacao.ModuloAutenticacao.Services;

public class JwtProvider : ITokenProvider
{
    private readonly string? chaveJwt;
    private readonly DateTime expiracaoJwt;
    private readonly string? audienciaValida;
    private readonly UserManager<Usuario> userManager;
    private readonly IRepositorioFuncionario _repositorioFuncionario;
    public JwtProvider(
        IConfiguration config,
        UserManager<Usuario> userManager,
        IRepositorioFuncionario repositorioFuncionario)
    {
        chaveJwt = config["JWT_GENERATION_KEY"];
        audienciaValida = config["JWT_AUDIENCE_DOMAIN"];
        this.userManager = userManager;
        _repositorioFuncionario = repositorioFuncionario;

        if (string.IsNullOrEmpty(chaveJwt))
            throw new ArgumentNullException(nameof(chaveJwt), "Chave de geração de token não configurada");

        if (string.IsNullOrEmpty(audienciaValida))
            throw new ArgumentNullException(nameof(audienciaValida), "Audiência válida para transmissão de tokens não configurada");

        expiracaoJwt = DateTime.Now.AddDays(3);
    }

    public IAccessToken GerarAccessToken(Usuario usuario)
    {
        return GerarAccessTokenAsync(usuario).GetAwaiter().GetResult();
    }

    private async Task<IAccessToken> GerarAccessTokenAsync(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var chaveEmBytes = Encoding.ASCII.GetBytes(chaveJwt!);

        var roles = await userManager.GetRolesAsync(usuario);
        var primeiraRole = roles.FirstOrDefault();
        var cargo = MapRoleStringToEnum(primeiraRole);

        var funcionario = await _repositorioFuncionario.SelecionarTodosSemFiltroAsync(usuario.Id);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName!)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        if (funcionario is not null)
        {
            claims.Add(new Claim("FuncionarioId", funcionario.Id.ToString()));
            claims.Add(new Claim("EmpresaId", funcionario.AdminId.ToString()));
        }
        else
        {
            claims.Add(new Claim("EmpresaId", usuario.Id.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "BarberClub",
            Audience = audienciaValida,
            Subject = new ClaimsIdentity(claims),
            Expires = expiracaoJwt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(chaveEmBytes),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenStr = tokenHandler.WriteToken(token);

        return new TokenResponse()
        {
            Chave = tokenStr,
            expiracaoToken = expiracaoJwt,
            Usuario = new UsuarioAutenticadoDto
            {
                Id = usuario.Id,
                Email = usuario.Email!,
                UserName = usuario.UserName!,
                NomeApresentacao = usuario.NomeApresentacao,
                Role = cargo,
                FuncionarioId = funcionario?.Id,
                EmpresaId = funcionario?.AdminId ?? usuario.Id
            }
        };
    }

    private static EnumCargo MapRoleStringToEnum(string? role)
    {
        return role switch
        {
            "Administrador" => EnumCargo.Administrador,
            "Funcionario" => EnumCargo.Funcionario,
            _ => EnumCargo.Administrador
        };
    }
}

