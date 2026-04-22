using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Enums;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Exceptions;

namespace TemplateApi.Api.Filters;

public class AuthenticatedAsAdminFilter(
    IAccessTokenValidator accessTokenValidator,
    IUserReadOnlyRepository repository
) : AuthenticatedUserFilter(accessTokenValidator, repository)
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;

    public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        await base.OnAuthorizationAsync(context);

        if (context.Result != null) return; // já falhou na autenticação

        var token = TokenOnRequest(context);
        var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);
        var user = await _repository.GetByIdentifier(userIdentifier);

        if (user is null || user.Role != Roles.ADMIN)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }
}