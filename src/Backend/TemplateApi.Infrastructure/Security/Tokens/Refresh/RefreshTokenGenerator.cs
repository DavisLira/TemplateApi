using TemplateApi.Domain.Security.Tokens;

namespace TemplateApi.Infrastructure.Security.Tokens.Refresh;
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}