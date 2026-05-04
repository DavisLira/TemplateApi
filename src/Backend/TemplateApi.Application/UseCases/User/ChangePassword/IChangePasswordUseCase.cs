using TemplateApi.Communication.Requests;

namespace TemplateApi.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}