using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TemplateApi.Infrastructure.Security.Tokens.Access;

public abstract class JwtTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var key = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(key);
    }
}