using Board.Application.Repositories;
using Board.Domain.Options;
using Board.Infrastructure.Data;
using Board.Infrastructure.Data.Repositories.Implementations;
using Board.ServiceDefaults;
using FastEndpoints;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedAppSettings(args);
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();

// Register Application validators and handlers
builder.Services.AddValidatorsFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardValidator).Assembly);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardCommand).Assembly));

// Infrastructure services
builder.Services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(builder.Configuration);
builder.AddDatabase<BoardDbContext, ConnectionStringsOptions>(x => x.BoardDbConnectionString);

// Repository registrations
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardItemRepository, BoardItemRepository>();
builder.Services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseFastEndpoints();
app.MapControllers();

app.Run();
