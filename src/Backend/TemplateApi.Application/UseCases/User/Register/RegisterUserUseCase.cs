using FluentValidation.Results;
using Mapster;
using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;
using TemplateApi.Domain.Repositories;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Security.Cryptography;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserWriteOnlyRepository writeOnlyRepository,
    IUserReadOnlyRepository readOnlyRepository,
    IPasswordEncrypter passwordEncrypter,
    IUnitOfWork unitOfWork
) : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter = passwordEncrypter;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        
        var user = request.Adapt<Domain.Entities.User>();
        user.Password = _passwordEncrypter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);
        await _unitOfWork.Commit();
        return new ResponseRegisterUserJson
        {
            Name = user.Name
        };
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