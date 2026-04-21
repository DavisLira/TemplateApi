using Moq;
using TemplateApi.Domain.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncrypter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncrypter>();

        _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!Password1");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if(!string.IsNullOrWhiteSpace(password))
        {
            _mock.Setup(passwordEncrypter => passwordEncrypter.IsValid(password, It.IsAny<string>())).Returns(true);
        }
        
        return this;
    }

    public IPasswordEncrypter Build() => _mock.Object;
}