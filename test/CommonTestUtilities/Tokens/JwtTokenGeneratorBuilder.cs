using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationMinutes: 5, signingKey: "abcdefghijklmnopqrstuvwxyz123456");
}