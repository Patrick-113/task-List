using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TodoListApi.Authentication
{
  public class TokenService
  {
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GenerateToken(string username)
    {
      //Recupera a configuração do Jwt
      var jwtSettings = _configuration.GetSection("Jwt");
      var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

      //Array de claims, informações e metadata sobre uma entidade, nesse caso o usuario.
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim("role", "User"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      //Criação de credenciais do token usando a chave
      var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

      //Configuração do token
      var token = new JwtSecurityToken(
        issuer: jwtSettings["Issuer"],
        audience: jwtSettings["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}