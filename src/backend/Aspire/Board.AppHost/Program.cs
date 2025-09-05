using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("sql-password", true);

// Add SQL Server
#if DEBUG
int sqlPort = 1435;
#else
int sqlPort = 1433;
#endif

var boardDb = builder
    .AddSqlServer("BoardDb", sqlPassword, port: sqlPort)
    .WithImageTag("2022-latest")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("BoardDbConnectionString", "BoardDb");

// Add migration service for project database
var projectDbMigrator = builder.AddProject<Projects.BoardDb_MigrationService>("boarddb-migrator")
    .WithReference(boardDb);

// Add API
var boardApi = builder.AddProject<Projects.Board_Api>("board-api")
    .WithReference(boardDb);

// Add Frontend
builder.AddNpmApp("board-web", "../../../frontend/Board.Web")
    .WithReference(boardApi)
    .WithHttpEndpoint(port: 3001, targetPort: 4200, name: "frontend", env: "PORT");
    //.PublishAsDockerFile();

builder.Build().Run();
