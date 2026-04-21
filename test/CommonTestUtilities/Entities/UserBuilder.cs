using Bogus;
using CommonTestUtilities.Cryptography;
using TemplateApi.Domain.Entities;
using TemplateApi.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build(string role = Roles.MEMBER)
    {
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var password = new Faker().Internet.Password(prefix: "!Aa1");

        var user = new Faker<User>()
            .RuleFor(u => u.UserId, _ => 1)
            .RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
            .RuleFor(u => u.Password, _ => passwordEncrypter.Encrypt(password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid())
            .RuleFor(u => u.Role, _ => role);

        return (user, password);
    }
}