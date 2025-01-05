using Ballastlane.Authentications;
using Ballastlane.Users.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ballastlane.Users.Application.Token;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly JwtTokenConfiguration _configuration;
    private readonly SigningCredentials _credentials;

    public JwtTokenService(JwtTokenConfiguration configuration)
    {
        _configuration = configuration;
        _credentials = new SigningCredentials(new SymmetricSecurityKey(configuration.GetKey()), SecurityAlgorithms.HmacSha256);
    }

    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration.Issuer,
            Audience = _configuration.Audience,
            SigningCredentials = _credentials
        };

        var token = _handler.CreateToken(tokenDescriptor);
        return _handler.WriteToken(token);
    }
}
