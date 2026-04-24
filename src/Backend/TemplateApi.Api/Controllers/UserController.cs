using Microsoft.AspNetCore.Mvc;
using TemplateApi.Api.Attributes;
using TemplateApi.Application.UseCases.User.Profile;
using TemplateApi.Application.UseCases.User.Register;
using TemplateApi.Application.UseCases.User.Update;
using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;

namespace TemplateApi.Api.Controllers;

public class UserController : TemplateApiBaseController
{
    [HttpPost]
    // [AuthenticatedAsAdmin]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpGet]
    [AuthenticatedUser]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile(
        [FromServices] IGetUserProfileUseCase useCase
    )
    {
        var result = await useCase.Execute();
        return Ok(result);
    }

    [HttpPut]
    [AuthenticatedUser]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] RequestUpdateUserJson request
    )
    {
        await useCase.Execute(request);
        return NoContent();
    }
}
