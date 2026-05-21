using Microsoft.AspNetCore.Mvc;
using TemplateApi.Application.UseCases.Token.RefreshToken;
using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;

namespace TemplateApi.Api.Controllers;

public class TokenController : TemplateApiBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUserRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request
    )
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}