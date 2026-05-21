using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Repositories;
using TemplateApi.Domain.Repositories.Token;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Security.Cryptography;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    ITokenRepository tokenRepository,
    IUnitOfWork unitOfWork
) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IPasswordEncrypter _passwordEncrypter = passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetByEmail(request.Email);
        var fakeHash = "$2a$11$nLHMM.i0A1Y/b8NETnHCeOf/fVFDGd6xiGFzYkjaK5.j0JK.m/oMm";
        var passwordHash = user?.Password ?? fakeHash;
        var passwordMatch = _passwordEncrypter.IsValid(request.Password, passwordHash);

        if (user == null || !passwordMatch) throw new InvalidLoginException();
        
        var refreshToken = await CreateAndSaveRefreshToken(user);

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = user.UserId
        };

        await _tokenRepository.SaveNewRefreshToken(refreshToken);

        await _unitOfWork.Commit();

        return refreshToken.Value;
    }
}