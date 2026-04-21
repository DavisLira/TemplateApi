using TemplateApi.Domain.Entities;

namespace TemplateApi.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}