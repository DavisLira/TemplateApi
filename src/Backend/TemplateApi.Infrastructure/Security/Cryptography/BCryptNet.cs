using TemplateApi.Domain.Security.Cryptography;

namespace TemplateApi.Infrastructure.Security.Cryptography;
public class BCryptNet : IPasswordEncrypter
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool IsValid(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}