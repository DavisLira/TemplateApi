using Microsoft.AspNetCore.Mvc;
using TemplateApi.Api.Filters;

namespace TemplateApi.Api.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}