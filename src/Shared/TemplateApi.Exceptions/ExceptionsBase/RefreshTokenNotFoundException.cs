using System.Net;

namespace TemplateApi.Exceptions.ExceptionsBase;
public class RefreshTokenNotFoundException() : TemplateApiException(ResourceMessagesException.EXPIRED_SESSION)
{
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;

    public override IList<string> GetErrors() => [Message];
}