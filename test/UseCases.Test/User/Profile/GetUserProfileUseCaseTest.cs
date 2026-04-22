using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using Shouldly;
using TemplateApi.Application.UseCases.User.Profile;

namespace UseCases.Test.User.Profile;

public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
    }

    private GetUserProfileUseCase CreateUseCase(TemplateApi.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        return new GetUserProfileUseCase(loggedUser);
    }
}