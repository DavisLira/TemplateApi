using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TemplateApi.Domain.Security.Tokens;

namespace TemplateApi.Infrastructure.Security.Tokens.Access.Generator;

public class JwtTokenGenerator(
    uint expirationMinutes,
    string signingKey
) : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly uint _expirationMinutes = expirationMinutes;
    private readonly string _signingKey = signingKey;

    public string Generate(Guid userIdentifier)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Sid, userIdentifier.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}