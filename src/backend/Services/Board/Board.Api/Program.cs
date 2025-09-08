using Board.Application.Interfaces;
using Board.Domain.Entities;
using Board.Domain.Options;
using Board.Infrastructure.Data;
using Board.Infrastructure.Data.Repositories;
using Board.ServiceDefaults;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

builder.AddSharedAppSettings(args);
builder.AddServiceDefaults();

// Add services to the container.
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddFastEndpoints();
services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "My API";
        s.Version = "v1";
    };
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();

// CORS allow all (development)
const string AllowAllCorsPolicy = "AllowAll";
builder.Services.AddCors(options =>
{
	options.AddPolicy(AllowAllCorsPolicy, policy =>
		policy
			.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod()
	);
});

// Register Application validators and handlers
services.AddValidatorsFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardValidator).Assembly);
services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardCommand).Assembly));

// Infrastructure services
services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(builder.Configuration);
services.AddOptionsWithBaseValidationOnStart<AuthOptions>(builder.Configuration, x => x.IsBypassAuthorization);
builder.AddDatabase<BoardDbContext, ConnectionStringsOptions>(x => x.BoardDbConnectionString);

services.AddCors(x =>
{
    x.AddDefaultPolicy(c =>
    {
        c.AllowAnyMethod();
        c.AllowAnyOrigin();
        c.AllowAnyHeader();
    });
});

//Add authentication
var authOptions = builder.Configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>() ?? new AuthOptions();
if (authOptions.IsBypassAuthorization)
{
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Bypass: treat any request as authenticated by injecting a fake token principal
                if (context.Principal == null)
                {
                    var claims = new[]
                    {
                        new System.Security.Claims.Claim("sub", "bypass-user"),
                        new System.Security.Claims.Claim("name", "Bypass User"),
                        new System.Security.Claims.Claim("permissions", "Editor")
                    };
                    var identity = new System.Security.Claims.ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                    context.Principal = new System.Security.Claims.ClaimsPrincipal(identity);
                }
                return Task.CompletedTask;
            }
        };
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false
        };
    });
}
else
{
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("Auth:Authority");
        options.Audience = builder.Configuration.GetValue<string>("Auth:Audience");
    });
}

//TO DO add global admin policy
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Editor", p => p.
        RequireAuthenticatedUser().
        RequireClaim("permissions", "Editor"));
});

services.AddHttpContextAccessor();
builder.Services.AddScoped<IRepository<Board.Domain.Entities.Board>, Repository<Board.Domain.Entities.Board>>();
builder.Services.AddScoped<IRepository<BoardItem>, Repository<BoardItem>>();
builder.Services.AddScoped<IRepository<BoardColumn>, Repository<BoardColumn>>();
builder.Services.AddScoped<IRepository<Tag>, Repository<Tag>>();

WebApplication app = builder.Build();

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
			var claims = new[]
			{
				new System.Security.Claims.Claim("sub", "bypass-user"),
				new System.Security.Claims.Claim("name", "Bypass User"),
				new System.Security.Claims.Claim("permissions", "Editor")
			};
			var identity = new System.Security.Claims.ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
			context.User = new System.Security.Claims.ClaimsPrincipal(identity);
		}
		await next();
	});
}
app.UseAuthorization();
app.UseFastEndpoints();
app.MapControllers();

app.Run();
