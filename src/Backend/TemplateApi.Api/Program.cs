using TemplateApi.Api.Filters;
using TemplateApi.Api.Middleware;
using TemplateApi.Application;
using TemplateApi.Application.Services.Mappings;
using TemplateApi.Infrastructure;
using TemplateApi.Infrastructure.Extensions;
using TemplateApi.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

MapConfigurations.Configure();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if(!builder.Configuration.IsTestEnvironment())
    await MigrateDatabase();

app.Run();

async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DatabaseMigration.Migrate(scope.ServiceProvider);
}