namespace TemplateApi.Domain.Security.Tokens;

public interface IAccessTokenValidator
{
    public Guid ValidateAndGetUserIdentifier(string token);
}