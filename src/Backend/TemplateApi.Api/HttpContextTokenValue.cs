using TemplateApi.Domain.Security.Tokens;

namespace TemplateApi.Api;

public class HttpContextTokenValue(
    IHttpContextAccessor contextAccessor
) : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public string Value()
    {
        var authentication = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authentication["Bearer ".Length..].Trim();
    }
}