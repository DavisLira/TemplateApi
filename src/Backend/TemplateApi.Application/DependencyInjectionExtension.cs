using Microsoft.Extensions.DependencyInjection;
using TemplateApi.Application.Services.Cryptography;
using TemplateApi.Application.UseCases.User.Register;

namespace TemplateApi.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddPasswordEncrypter(services);
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }

    private static void AddPasswordEncrypter(IServiceCollection services)
    {
        services.AddScoped(option => new PasswordEncrypter());
    }
}