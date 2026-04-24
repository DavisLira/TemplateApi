using TemplateApi.Communication.Requests;

namespace TemplateApi.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
    
}