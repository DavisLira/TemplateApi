using System.Net;

namespace TemplateApi.Exceptions.ExceptionsBase;

public abstract class TemplateApiException(string message) : SystemException(message)
{
    public abstract HttpStatusCode GetStatusCode();
    public abstract IList<string> GetErrors();
}