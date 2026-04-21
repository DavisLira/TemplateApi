using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;

namespace TemplateApi.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
}