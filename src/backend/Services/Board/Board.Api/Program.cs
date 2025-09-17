using Board.Api.Configuration;
using Board.Api.Features.Board.CreateBoard;
using Board.Application.DI;
using Board.Application.Mapping;
using Board.Domain.Options;
using Board.Infrastructure;
using Board.Infrastructure.Data;
using Board.ServiceDefaults;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

const string AllowAllCorsPolicy = "AllowAll";

//options registration
services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(configuration);
services.AddOptionsWithBaseValidationOnStart<AuthOptions>(configuration, x => x.IsBypassAuthorization);

AuthOptions authOptions = configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>() ?? new AuthOptions();

builder.AddSharedAppSettings(args);
builder.AddServiceDefaults();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
services.AddControllers();

// Register Application validators and handlers
services.AddValidatorsFromAssembly(typeof(CreateBoardValidator).Assembly);
services.AddAutoMapper(typeof(BoardMappingProfile).Assembly);

builder.AddDatabase<BoardDbContext, ConnectionStringsOptions>(x => x.BoardDbConnectionString);
services.ConfigureAuth(authOptions)
    .ConfigureRepositories(configuration)
    .ConfigureAuth(AllowAllCorsPolicy)
    .AddEndpointsApiExplorer()
    .AddFastEndpoints()
    .ConfigureSwagger(configuration)
    .AddHttpContextAccessor()
    .AddInfrastructure(configuration)
    .ConfigureApplication();

WebApplication app = builder.Build();

var supportedCultures = new[] { "en-US", "ru-RU" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

// Only redirect to HTTPS outside Development to avoid CORS issues on redirects
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(AllowAllCorsPolicy);
app.UseAuthentication();
if (authOptions.IsBypassAuthorization)
{
    app.Use(async (context, next) =>
    {
        if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            System.Security.Claims.Claim[] claims = new[]
            {
                new System.Security.Claims.Claim("sub", "bypass-user"),
                new System.Security.Claims.Claim("name", "Bypass User"),
                new System.Security.Claims.Claim("permissions", "Editor")
            };
            System.Security.Claims.ClaimsIdentity identity = new System.Security.Claims.ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            context.User = new System.Security.Claims.ClaimsPrincipal(identity);
        }
        await next();
    });
}
app.UseAuthorization();
app.UseFastEndpoints();
app.MapControllers();

app.Run();
