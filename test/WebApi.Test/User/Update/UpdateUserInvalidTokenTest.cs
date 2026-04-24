using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TemplateApi.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest(
    CustomWebApplicationFactory factory
) : TemplateApiClassFixture(factory)
{
    private const string METHOD = "user";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoPut(METHOD, request, token: "tokenInvalid");
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoPut(METHOD, request, token: string.Empty);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_Not_Found()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var response = await DoPut(METHOD, request, token);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}