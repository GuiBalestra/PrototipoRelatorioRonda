using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrototipoRelatorioRonda.Application.Interfaces;
using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrototipoRelatorioRonda.API.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthController(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Realiza o login do usuário e retorna um token JWT.
    /// </summary>
    /// <param name="loginDto">Credenciais do usuário (email e senha)</param>
    /// <returns>Token JWT para autenticação</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
        if (usuario == null)
            return Unauthorized("Email ou senha inválidos");

        var hashSenha = HashPassword(loginDto.Senha);
        if (hashSenha != usuario.HashSenha)
            return Unauthorized("Email ou senha inválidos");

        var token = GenerateJwtToken(usuario);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo,
            usuario = new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                Funcao = usuario.Funcao.ToString(),
                Empresa = usuario.Empresa?.Nome
            }
        });
    }

    private JwtSecurityToken GenerateJwtToken(Usuario usuario)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "ChaveSecretaTemporariaParaTestes123456789");
        var issuer = _configuration["Jwt:Issuer"] ?? "PrototipoRelatorioRonda";
        var audience = _configuration["Jwt:Audience"] ?? "PrototipoRelatorioRonda";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Funcao.ToString())
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
