using TemplateApi.Domain.Security.Cryptography;
using TemplateApi.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build() => new BCryptNet();
}