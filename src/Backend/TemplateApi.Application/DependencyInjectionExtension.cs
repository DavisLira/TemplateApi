using Microsoft.Extensions.DependencyInjection;
using TemplateApi.Application.Services.Cryptography;
using TemplateApi.Application.UseCases.User.Register;

namespace TemplateApi.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddPasswordEncripter(services);
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }

    private static void AddPasswordEncripter(IServiceCollection services)
    {
        services.AddScoped(option => new PasswordEncripter());
    }
}