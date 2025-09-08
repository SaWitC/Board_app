using Board.Application.Interfaces;
using Board.Domain.Entities;
using Board.Domain.Options;
using Board.Infrastructure.Data;
using Board.Infrastructure.Data.Repositories;
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
builder.Services.AddValidatorsFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardValidator).Assembly);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Board.Application.Commands.CreateBoard.CreateBoardCommand).Assembly));

// Infrastructure services
builder.Services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(builder.Configuration);
builder.AddDatabase<BoardDbContext, ConnectionStringsOptions>(x => x.BoardDbConnectionString);

// Repository registrations
builder.Services.AddScoped<IRepository<Board.Domain.Entities.Board>, Repository<Board.Domain.Entities.Board>>();
builder.Services.AddScoped<IRepository<BoardItem>, Repository<BoardItem>>();
builder.Services.AddScoped<IRepository<BoardColumn>, Repository<BoardColumn>>();
builder.Services.AddScoped<IRepository<Tag>, Repository<Tag>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Only redirect to HTTPS outside Development to avoid CORS issues on redirects
if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

app.UseCors(AllowAllCorsPolicy);
app.UseAuthorization();
app.UseFastEndpoints();
app.MapControllers();

app.Run();
