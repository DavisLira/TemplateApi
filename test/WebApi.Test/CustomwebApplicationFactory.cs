using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TemplateApi.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;
using TemplateApi.Domain.Security.Cryptography;
using UserEntitie = TemplateApi.Domain.Entities.User;
using CommonTestUtilities.Entities;
using TemplateApi.Domain.Enums;

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
                var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
                var dbContext = scope.ServiceProvider.GetRequiredService<TemplateApiDbContext>();

                StartDatabase(dbContext, passwordEncrypter);
            });
    }

    private void StartDatabase(
        TemplateApiDbContext dbContext,
        IPasswordEncrypter passwordEncrypter)
    {
        var userMember = AddUserMember(dbContext, passwordEncrypter);
        var userAdmin = AddUserAdmin(dbContext, passwordEncrypter);
        dbContext.SaveChanges();
    }

    private UserEntitie AddUserMember(
        TemplateApiDbContext dbContext, 
        IPasswordEncrypter passwordEncrypter)
    {
        var user = UserBuilder.Build();
        user.UserId = 1;
        var password = user.Password;
        user.Password = passwordEncrypter.Encrypt(user.Password);
        dbContext.Users.Add(user);

        User_Member = new UserIdentityManager(user, password);
        return user;
    }

    private UserEntitie AddUserAdmin(
        TemplateApiDbContext dbContext, 
        IPasswordEncrypter passwordEncrypter)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.UserId = 2;
        var password = user.Password;
        user.Password = passwordEncrypter.Encrypt(user.Password);
        dbContext.Users.Add(user);

        User_Member = new UserIdentityManager(user, password);
        return user;
    }
}