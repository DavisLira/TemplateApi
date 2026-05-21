using FluentValidation.Results;
using Mapster;
using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Entities;
using TemplateApi.Domain.Repositories;
using TemplateApi.Domain.Repositories.Token;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Security.Cryptography;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserWriteOnlyRepository writeOnlyRepository,
    IUserReadOnlyRepository readOnlyRepository,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator accessTokenGenerator,
    IUnitOfWork unitOfWork,
    ITokenRepository tokenRepository,
    IRefreshTokenGenerator refreshTokenGenerator
) : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter = passwordEncrypter;

    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        
        var user = request.Adapt<Domain.Entities.User>();
        user.Password = _passwordEncrypter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        var accessToken = _accessTokenGenerator.Generate(user.UserIdentifier);

        await _writeOnlyRepository.Add(user);
        await _unitOfWork.Commit();

        var refreshToken = await CreateAndSaveRefreshToken(user);

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = _refreshTokenGenerator.Generate();

        await _tokenRepository.SaveNewRefreshToken(new RefreshToken
        {
            Value = refreshToken,
            UserId = user.UserId
        });

        await _unitOfWork.Commit();

        return refreshToken;
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExists = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}