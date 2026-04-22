using UserEntity = TemplateApi.Domain.Entities.User;

namespace WebApi.Test.Resources;

public class UserIdentityManager(
    UserEntity user,
    string password,
    string token
)
{
    private readonly UserEntity _user = user;
    private readonly string _password = password;
    private readonly string _token = token;
    
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetToken() => _token;
    public Guid GetUserIdentifier() => _user.UserIdentifier;
}