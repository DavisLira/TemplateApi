using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Repositories;
using TemplateApi.Domain.Repositories.Token;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Domain.ValueObjects;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Application.UseCases.Token.RefreshToken;

public class UserRefreshTokenUseCase(
    IUnitOfWork unitOfWork,
    ITokenRepository tokenRepository,
    IRefreshTokenGenerator refreshTokenGenerator,
    IAccessTokenGenerator accessTokenGenerator) : IUserRefreshTokenUseCase
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;

    public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        var refreshToken = await _tokenRepository.Get(request.RefreshToken);

        if(refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedAt.AddDays(TemplateApiRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
        if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

        await _unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier),
            RefreshToken = newRefreshToken.Value
        };
    }
}