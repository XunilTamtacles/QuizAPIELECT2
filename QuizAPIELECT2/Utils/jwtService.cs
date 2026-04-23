namespace QuizAPIELECT2.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class jwtService
    {
        private readonly IConfiguration _configuration;

    public jwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
     
    public string GenerateToken(string username, string role)
    {
        var secretKey = _configuration["Jwt:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim("role", role),
            new Claim("username", username)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );


        return new JwtSecurityTokenHandler().WriteToken(token);

    }
 }    

