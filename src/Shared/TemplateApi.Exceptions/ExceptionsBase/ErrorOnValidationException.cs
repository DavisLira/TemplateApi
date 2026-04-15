namespace TemplateApi.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(
    IList<string> errorMessages
) : TemplateApiException
{
    public IList<string> ErrorMessages { get; set; } = errorMessages;

}