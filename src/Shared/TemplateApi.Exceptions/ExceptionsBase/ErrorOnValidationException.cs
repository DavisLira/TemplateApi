using System.Net;

namespace TemplateApi.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(
    IList<string> errorMessages
) : TemplateApiException(string.Empty)
{
    private readonly IList<string> _errorMessages = errorMessages;
    public override IList<string> GetErrors() => _errorMessages;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}