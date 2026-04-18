using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TemplateApi.Infrastructure.DataAccess;

namespace TemplateApi.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static async Task Migrate(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<TemplateApiDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}