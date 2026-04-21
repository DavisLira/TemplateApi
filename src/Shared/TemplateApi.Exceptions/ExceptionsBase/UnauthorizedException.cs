using System.Net;

namespace TemplateApi.Exceptions.ExceptionsBase;

public class UnauthorizedException(string message) : TemplateApiException(message)
{
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;

    public override List<string> GetErrors() => [Message];
}