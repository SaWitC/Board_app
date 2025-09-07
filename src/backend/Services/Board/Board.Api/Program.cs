using Board.Application.Repositories;
using Board.Domain.Options;
using Board.Infrastructure.Data;
using Board.Infrastructure.Data.Repositories.Implementations;
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
// Register Application validators and handlers
services.AddValidatorsFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardValidator).Assembly);
services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardCommand).Assembly));

// Infrastructure services
services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(builder.Configuration);
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

// Repository registrations
services.AddScoped<IBoardRepository, BoardRepository>();
services.AddScoped<IBoardItemRepository, BoardItemRepository>();
services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
services.AddScoped<ITagRepository, TagRepository>();

//Add authentication
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

//TO DO add global admin policy
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Editor", p => p.
        RequireAuthenticatedUser().
        RequireClaim("permissions", "Editor"));
});

services.AddHttpContextAccessor();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}


app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors();

app.UseFastEndpoints();
app.MapControllers();

app.Run();
