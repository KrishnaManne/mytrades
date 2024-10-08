using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration configuration;
    private readonly SymmetricSecurityKey securityKey;
    private readonly SigningCredentials signingCredentials;
    private readonly string? issuer;
    private readonly string? audience;
    public JwtTokenGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
        var jwtSettings = configuration.GetSection("Jwt");
        var jwtSecurityKey = jwtSettings.GetValue<string>("Key");
        issuer = jwtSettings.GetValue<string>("Issuer");
        audience = jwtSettings.GetValue<string>("Audience");
        Console.WriteLine($"Key: {jwtSecurityKey}");
        securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey));
        signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }
    public string GenerateJwtToken(Guid userId, string userName, string email, int expiresInMinutes)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, userName),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiresInMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(Guid userId, string userName, string email, int expiresInMinutes);
}