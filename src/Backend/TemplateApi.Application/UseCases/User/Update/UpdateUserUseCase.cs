using FluentValidation.Results;
using TemplateApi.Communication.Requests;
using TemplateApi.Domain.Repositories;
using TemplateApi.Domain.Repositories.User;
using TemplateApi.Domain.Services.LoggedUser;
using TemplateApi.Exceptions;
using TemplateApi.Exceptions.ExceptionsBase;

namespace TemplateApi.Application.UseCases.User.Update;

public class UpdateUserUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository updateOnlyRepository,
    IUserReadOnlyRepository readOnlyRepository,
    IUnitOfWork unitOfWork
) : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();
        await Validate(request, loggedUser.Email);

        var user = await _updateOnlyRepository.GetById(loggedUser.UserId);
        user.Name = request.Name;
        user.Email = request.Email;

        _updateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var result = validator.Validate(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExist)
            {
                result.Errors.Add(new ValidationFailure(
                    "email",
                    ResourceMessagesException.EMAIL_ALREADY_REGISTERED
                ));
            }
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}