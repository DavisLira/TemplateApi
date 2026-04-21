using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TemplateApi.Communication.Responses;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TemplateApiException exception)
            HandleProjectException(exception, context);
        else
            ThrowUnkwonException(context);
    }

    private static void HandleProjectException(TemplateApiException exception, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)exception.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(exception.GetErrors()));
    }

    private static void ThrowUnkwonException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }
}