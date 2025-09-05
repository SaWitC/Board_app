using Board.Domain.Options;
using Board.Infrastructure.Data;
using Board.ServiceDefaults;
using BoardDb.MigrationService.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.SetupDefaults(args);
builder.Services.AddOptionsWithBaseValidationOnStart<ConnectionStringsOptions>(builder.Configuration);
builder.AddDatabase<BoardDbContext, ConnectionStringsOptions>(x => x.BoardDbConnectionString, "Board.Infrastructure");

// Register our new service as Hosted Service
builder.Services.AddHostedService<BoardDbMigrationService>();

var host = builder.Build();
host.Run();
