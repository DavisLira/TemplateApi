namespace TemplateApi.Exceptions.ExceptionsBase;

public abstract class TemplateApiException(string message) : SystemException(message)
{
    public abstract int StatusCode { get; }
    public abstract IList<string> GetErrors();
}