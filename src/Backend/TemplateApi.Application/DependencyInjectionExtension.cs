using Microsoft.Extensions.DependencyInjection;
using TemplateApi.Application.UseCases.Login.DoLogin;
using TemplateApi.Application.UseCases.User.ChangePassword;
using TemplateApi.Application.UseCases.User.Profile;
using TemplateApi.Application.UseCases.User.Register;
using TemplateApi.Application.UseCases.User.Update;

namespace TemplateApi.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }
}