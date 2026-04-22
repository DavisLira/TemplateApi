using Mapster;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Services.LoggedUser;

namespace TemplateApi.Application.UseCases.User.Profile;

public class GetUserProfileUseCase(
    ILoggedUser loggedUser
) : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();
        return user.Adapt<ResponseUserProfileJson>();
    }
}