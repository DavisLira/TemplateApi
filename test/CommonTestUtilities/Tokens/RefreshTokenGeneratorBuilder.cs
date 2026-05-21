using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Infrastructure.Security.Tokens.Refresh;

namespace CommonTestUtilities.Tokens;
public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}