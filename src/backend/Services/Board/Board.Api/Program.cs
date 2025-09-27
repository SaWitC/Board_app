using Board.Api.Configuration;
using Board.Api.Extensions;
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

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

const string AllowAllCorsPolicy = "AllowAll";

//options registration
services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(configuration);
services.AddOptionsWithBaseValidationOnStart<AuthOptions>(configuration);

AuthOptions authOptions = configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>() ?? throw new InvalidOperationException("Auth options are not configured");

builder.AddSharedAppSettings(args);
builder.AddServiceDefaults();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
services.AddControllers();

// Register Application validators and handlers
services.AddValidatorsFromAssembly(typeof(CreateBoardValidator).Assembly);
services.AddAutoMapper(typeof(BoardMappingProfile).Assembly);

builder.AddDatabase<BoardDbContext, ConnectionStringsOptions>(x => x.BoardDbConnectionString, "Board.Infrastructure");
services.ConfigureAuth(authOptions)
    .ConfigureRepositories(configuration)
    .ConfigureAuth(AllowAllCorsPolicy)
    .AddEndpointsApiExplorer()
    .AddFastEndpoints()
    .ConfigureSwagger(configuration)
    .AddHttpContextAccessor()
    .AddInfrastructure(configuration)
    .ConfigureApplication();

// ProblemDetails + Exception handling
services.AddProblemDetails();
services.AddExceptionHandler<GlobalExceptionHandler>();

WebApplication app = builder.Build();

app.MigrateDatabase();

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

// Enable centralized exception handling with ProblemDetails
app.UseExceptionHandler();

// ProblemDetails for non-exception status codes
app.UseProblemDetailsForStatusCodes();

app.UseCors(AllowAllCorsPolicy);
app.UseAuthentication();

app.UseAuthorization();

// Configure FastEndpoints to emit RFC7807 and return all validation failures
app.UseFastEndpoints(cfg =>
{
    cfg.Errors.UseProblemDetails();
});

app.MapControllers();

app.Run();
