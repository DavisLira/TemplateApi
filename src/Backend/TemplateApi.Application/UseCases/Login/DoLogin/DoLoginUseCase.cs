using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Security.Cryptography;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator accessTokenGenerator
) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IPasswordEncrypter _passwordEncrypter = passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetByEmail(request.Email);
        var fakeHash = "$2a$11$nLHMM.i0A1Y/b8NETnHCeOf/fVFDGd6xiGFzYkjaK5.j0JK.m/oMm";
        var passwordHash = user?.Password ?? fakeHash;
        var passwordMatch = _passwordEncrypter.IsValid(request.Password, passwordHash);

        if (user == null || !passwordMatch) throw new InvalidLoginException();
        
        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user)
            }
        };
    }
}