using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using TemplateApi.Communication.Requests;
using TemplateApi.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Shouldly;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordTest : TemplateApiClassFixture
{
    private const string METHOD = "user/change-password";

    private readonly string _password;
    private readonly string _email;
    private readonly string _token;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _password = factory.User_Member.GetPassword();
        _email = factory.User_Member.GetEmail();
        _token = factory.User_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var response = await DoPut(METHOD, request, _token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password,
        };

        response = await DoPost(requestUri: "login", request: loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost(requestUri: "login", request: loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Password_Different_Current_Password(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await DoPut(METHOD, request, token: _token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));
        errors.ShouldHaveSingleItem()
            .GetString()
            .ShouldBe(expectedMessage);
    }
}