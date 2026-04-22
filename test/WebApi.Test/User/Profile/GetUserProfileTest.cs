using System.Net;
using System.Text.Json;
using Shouldly;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : TemplateApiClassFixture
{
    private readonly string METHOD = "user";

    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;
    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = factory.User_Member.GetToken();
        _userName = factory.User_Member.GetName();
        _userEmail = factory.User_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet(METHOD, _token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_userName);
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_userEmail);
    }
}