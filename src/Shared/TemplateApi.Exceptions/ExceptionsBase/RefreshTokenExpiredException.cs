using System.Net;

namespace TemplateApi.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException() : TemplateApiException(ResourceMessagesException.INVALID_SESSION)
{
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;

    public override IList<string> GetErrors() => [Message];
}