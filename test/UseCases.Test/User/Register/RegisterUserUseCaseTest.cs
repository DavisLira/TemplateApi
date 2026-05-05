using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TemplateApi.Application.UseCases.User.Register;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Tokens.ShouldNotBeNull();
        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors()
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            readRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(writeRepository, readRepositoryBuilder.Build(), passwordEncrypter, accessTokenGenerator, unitOfWork);
    }
}