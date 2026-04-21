using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;
using TemplateApi.Application.UseCases.Login.DoLogin;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;
        request.Password = password;

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        user.Email = request.Email;
        var useCase = CreateUseCase(user);

        var act = async() => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<InvalidLoginException>();

        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        var act = async() => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
    }

    private static DoLoginUseCase CreateUseCase(TemplateApi.Domain.Entities.User user)
    {
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var repository = new UserReadOnlyRepositoryBuilder().GetByEmail(user).Build();

        return new DoLoginUseCase(repository, passwordEncrypter);
    }
}