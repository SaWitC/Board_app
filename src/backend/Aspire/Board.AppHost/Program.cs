var builder = DistributedApplication.CreateBuilder(args);

var pgUsername = builder.AddParameter("pg-username", true);
var pgPassword = builder.AddParameter("pg-password", true);

// Add Postgres
#if DEBUG
int pgPort = 5435;
#else
int pgPort = 5432;
#endif

var boardDb = builder
    .AddPostgres("postgres", pgUsername, pgPassword, port: pgPort)
    //.WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("BoardDbConnectionString", "BoardDb");

// Add migration service for project database
var projectDbMigrator = builder.AddProject<Projects.BoardDb_MigrationService>("boarddb-migrator")
    .WithReference(boardDb)
    .WaitFor(boardDb);

// Add API
var boardApi = builder.AddProject<Projects.Board_Api>("board-api")
    .WithReference(boardDb);

// Add Frontend
builder.AddNpmApp("board-web", "../../../frontend/Board.Web")
    .WithReference(boardApi)
    .WithHttpEndpoint(port: 4203, targetPort: 4200, name: "frontend", env: "PORT");
    //.PublishAsDockerFile();

builder.Build().Run();
