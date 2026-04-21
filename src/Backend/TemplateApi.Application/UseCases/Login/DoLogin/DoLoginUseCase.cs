using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Security.Cryptography;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IPasswordEncrypter passwordEncrypter
) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IPasswordEncrypter _passwordEncrypter = passwordEncrypter;

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetByEmail(request.Email);
        var fakeHash = "$2a$11$fa3Hfzee5Eg5Glio.4zVK.xp3A0ypfxBldLB9CFh7lHXnYq/X/ptG";
        var passwordHash = user?.Password ?? fakeHash;
        var passwordMatch = _passwordEncrypter.IsValid(request.Password, passwordHash);

        if (user == null || !passwordMatch) throw new InvalidLoginException();
        
        return new ResponseRegisterUserJson
        {
            Name = user.Name
        };
    }
}