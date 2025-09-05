using Board.Application.Repositories;
using Board.Domain.Options;
using Board.Infrastructure.Data;
using Board.Infrastructure.Data.Repositories.Implementations;
using Board.ServiceDefaults;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

builder.AddSharedAppSettings(args);
builder.AddServiceDefaults();

// Add services to the container.
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddFastEndpoints();

// Register Application validators and handlers
services.AddValidatorsFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardValidator).Assembly);
services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardCommand).Assembly));

// Infrastructure services
services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(builder.Configuration);
builder.AddDatabase<BoardDbContext, ConnectionStringsOptions>(x => x.BoardDbConnectionString);

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

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.UseFastEndpoints();
app.MapControllers();

app.Run();
