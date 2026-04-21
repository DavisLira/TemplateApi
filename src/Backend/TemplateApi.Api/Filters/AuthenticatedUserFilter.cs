using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Api.Filters;

public class AuthenticatedUserFilter(
    IAccessTokenValidator accessTokenValidator,
    IUserReadOnlyRepository repository
) : IAsyncAuthorizationFilter
{
        private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository = repository;
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);
            var exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);
            if (!exist)
            {
                throw new UnauthorizedException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
            {
                TokenIsExpired = true,
            });
        }
        catch (TemplateApiException exception)
        {
            context.HttpContext.Response.StatusCode = (int)exception.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(exception.GetErrors()));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication))
        {
            throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);
        }
        return authentication["Bearer ".Length..].Trim();
    }
}