using System.Net;

namespace TemplateApi.Exceptions.ExceptionsBase;

public class InvalidLoginException() : TemplateApiException(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
{
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;

    public override List<string> GetErrors() => [Message];
}