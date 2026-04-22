using TemplateApi.Communication.Responses;

namespace TemplateApi.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}