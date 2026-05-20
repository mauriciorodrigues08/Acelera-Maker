// Responsável por gerar e validar tokens JWT.
// O token carrega as informações do usuário autenticado
// É usado para proteger os endpoints da API.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogPessoal.Models;
using Microsoft.IdentityModel.Tokens;

namespace BlogPessoal.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, usuario.Email!),
            new Claim(ClaimTypes.Name, usuario.Nome!),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}