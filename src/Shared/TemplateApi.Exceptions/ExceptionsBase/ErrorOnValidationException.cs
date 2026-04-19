using System.Net;

namespace TemplateApi.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(
    IList<string> errorMessages
) : TemplateApiException(string.Empty)
{
    private readonly IList<string> _errors = errorMessages;
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override IList<string> GetErrors()
    {
        return _errors;
    }
}