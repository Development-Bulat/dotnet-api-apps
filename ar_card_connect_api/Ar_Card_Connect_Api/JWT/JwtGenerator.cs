using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ar_Card_Connect_Api.Models;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Ar_Card_Connect_Api.JWT;

public class JwtGenerator
{
    private readonly string _secretKey;
    private readonly string _issuer;
    public JwtGenerator(IConfiguration configuration)
    {
        _secretKey = configuration["JWT:Key"] ?? throw new Exception("JWT не найден");
        _issuer = configuration["JWT:Issuer"] ?? "ArCardConnect";
        
    }

    public string GenerateToken(UserProfile user)
    {
        var claims = new[]
        {
            new Claim("userId", user.user_id.ToString()),
            new Claim("roleId", user.role_id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}