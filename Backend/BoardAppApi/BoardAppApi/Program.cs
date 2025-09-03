using BoardAppApi.Data;
using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using BoardAppApi.Data.Repositories.Implemntations;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<BaseRepository<BoardItem>, BoardItemRepository>();
builder.Services.AddScoped<BaseRepository<Board>, BoardRepository>();
builder.Services.AddDbContext<DbContext>(options =>
    options.UseSqlServer("connectionString"));


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
