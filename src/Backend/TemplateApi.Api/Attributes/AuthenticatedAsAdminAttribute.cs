using Microsoft.AspNetCore.Mvc;
using TemplateApi.Api.Filters;

namespace TemplateApi.Api.Attributes;

public class AuthenticatedAsAdminAttribute : TypeFilterAttribute
{
    public AuthenticatedAsAdminAttribute() : base(typeof(AuthenticatedAsAdminFilter)) { }
}