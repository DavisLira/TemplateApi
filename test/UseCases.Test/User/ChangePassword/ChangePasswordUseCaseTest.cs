using TemplateApi.Application.UseCases.User.ChangePassword;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;
using TemplateApi.Exceptions.ExceptionsBase;
using TemplateApi.Exceptions;

namespace UseCases.Test.User.ChangePassword;
public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.INVALID_PASSWORD);
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
    }

    private static ChangePasswordUseCase CreateUseCase(TemplateApi.Domain.Entities.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, passwordEncrypter, userUpdateRepository, unitOfWork);
    }
}