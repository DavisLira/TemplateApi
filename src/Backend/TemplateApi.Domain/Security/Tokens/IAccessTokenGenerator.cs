using TemplateApi.Domain.Entities;

namespace TemplateApi.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier);
}