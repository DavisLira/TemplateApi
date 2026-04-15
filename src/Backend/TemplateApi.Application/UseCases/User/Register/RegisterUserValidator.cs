using FluentValidation;
using TemplateApi.Exceptions;
using TemplateApi.Communication.Requests;

namespace TemplateApi.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_INVALID)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(8)
            .WithMessage(ResourceMessagesException.PASSWORD_EMPTY);
    }
}