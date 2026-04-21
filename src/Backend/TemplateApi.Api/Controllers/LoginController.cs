using Microsoft.AspNetCore.Mvc;
using TemplateApi.Application.UseCases.Login.DoLogin;
using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;

namespace TemplateApi.Api.Controllers;

public class LoginController : TemplateApiBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase, 
        [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}