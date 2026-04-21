using Microsoft.AspNetCore.Mvc;
using TemplateApi.Application.UseCases.User.Register;
using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;

namespace TemplateApi.Api.Controllers;

public class UserController : TemplateApiBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}
