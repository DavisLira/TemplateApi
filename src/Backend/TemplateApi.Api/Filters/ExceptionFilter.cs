using System.Net;
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
        if (context.Exception is TemplateApiException)
            HandleProjectException(context);
        else
            ThrowUnkwonException(context);
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        var exception = (TemplateApiException)context.Exception;
        var errorResponse = new ResponseErrorJson(exception.GetErrors());
        context.HttpContext.Response.StatusCode = exception.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private static void ThrowUnkwonException(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}