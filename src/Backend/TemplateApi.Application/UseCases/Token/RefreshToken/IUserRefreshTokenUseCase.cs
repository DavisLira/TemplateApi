using TemplateApi.Communication.Requests;
using TemplateApi.Communication.Responses;

namespace TemplateApi.Application.UseCases.Token.RefreshToken;

public interface IUserRefreshTokenUseCase
{
    Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}