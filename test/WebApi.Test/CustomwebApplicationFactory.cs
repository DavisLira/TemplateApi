using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TemplateApi.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;
using TemplateApi.Domain.Security.Cryptography;
using UserEntity = TemplateApi.Domain.Entities.User;
using CommonTestUtilities.Entities;
using TemplateApi.Domain.Enums;
using TemplateApi.Domain.Security.Tokens;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public UserIdentityManager User_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<TemplateApiDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TemplateApiDbContext>();
                var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, accessTokenGenerator);
            });
    }

    private void StartDatabase(
        TemplateApiDbContext dbContext,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var userMember = AddUserMember(dbContext, accessTokenGenerator);
        var userAdmin = AddUserAdmin(dbContext, accessTokenGenerator);
        dbContext.SaveChanges();
    }

    private UserEntity AddUserMember(
        TemplateApiDbContext dbContext,
        IAccessTokenGenerator accessTokenGenerator)
    {
        (var user, var password) = UserBuilder.Build();
        user.UserId = 1;
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user.UserIdentifier);

        User_Member = new UserIdentityManager(user, password, token);
        return user;
    }

    private UserEntity AddUserAdmin(
        TemplateApiDbContext dbContext,
        IAccessTokenGenerator accessTokenGenerator)
    {
        (var user, var password) = UserBuilder.Build(Roles.ADMIN);
        user.UserId = 2;
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user.UserIdentifier);

        User_Admin = new UserIdentityManager(user, password, token);
        return user;
    }
}