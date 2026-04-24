using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TemplateApi.Application.UseCases.User.Register;
using TemplateApi.Application.UseCases.User.Update;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);
        
        await act.ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.NAME_EMPTY);

        user.Name.ShouldNotBe(request.Name);
        user.Email.ShouldNotBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);

        user.Name.ShouldNotBe(request.Name);
        user.Email.ShouldNotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(TemplateApi.Domain.Entities.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var updateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if (!string.IsNullOrWhiteSpace(email))
            readRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new UpdateUserUseCase(loggedUser, updateRepository, readRepositoryBuilder.Build(), unitOfWork);
    }
}