using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClassLibrary1DotNet8.MinimalApi.Shared.Services;

public class JwtTokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtTokenService(JwtSecurityTokenHandler tokenHandler,)
    {
        _tokenHandler = tokenHandler;
    }

    public string GenerateJwtToken(string userName,string email)
    {
        //var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
        var key = Encoding.ASCII.GetBytes("your_very_long_secret_key_1234567890");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ReadJwtToken(string token)
    {
        var key = Encoding.ASCII.GetBytes("your_very_long_secret_key_1234567890");
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        var principal =
            _tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        return principal;
    }
}