using Microsoft.OpenApi;
using TemplateApi.Api;
using TemplateApi.Api.Converters;
using TemplateApi.Api.Filters;
using TemplateApi.Api.Middleware;
using TemplateApi.Application;
using TemplateApi.Application.Services.Mappings;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Infrastructure;
using TemplateApi.Infrastructure.Extensions;
using TemplateApi.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            {
                "Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http, 
                    Scheme = "bearer",             
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Cole apenas o token JWT aqui (não precisa escrever 'Bearer' antes)."
                }
            }
        };

        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });

        return Task.CompletedTask;
    });
});

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

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

app.UseAuthentication();

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

public partial class Program
{
    protected Program() { }
}