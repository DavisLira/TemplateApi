using UserEntity = TemplateApi.Domain.Entities.User;

namespace WebApi.Test.Resources;

public class UserIdentityManager(
    UserEntity user,
    string password
)
{
    private readonly UserEntity _user = user;
    private readonly string _password = password;
    
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
}