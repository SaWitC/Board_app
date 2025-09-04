using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace BoardDb.MigrationService.Services;

/// <summary>
/// Base abstract class for database migration services
/// </summary>
/// <typeparam name="TContext">Database context type</typeparam>
public abstract class BaseMigrationService<TContext> : BackgroundService where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<BaseMigrationService<TContext>> _logger;

    /// <summary>
    /// Constructor for base migration service
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    /// <param name="hostApplicationLifetime">Application lifetime</param>
    /// <param name="logger">Logger</param>
    protected BaseMigrationService(
        IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<BaseMigrationService<TContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
    }

    /// <summary>
    /// Executes database migration
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    /// <returns>Execution task</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Migration service for {DbContextName} started.", typeof(TContext).Name);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            // Use the DbContext's configured connection string
            var connection = dbContext.Database.GetDbConnection();
            var connectionString = connection.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.LogError("Connection string is not configured for {DbContextName}.", typeof(TContext).Name);
                Environment.ExitCode = -1;
                return;
            }

            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;

            _logger.LogInformation("Starting migration for database: {DatabaseName}", databaseName);

            // Optional connectivity check (non-fatal)
            try
            {
                using var testConnection = new SqlConnection(connectionString);
                await testConnection.OpenAsync(stoppingToken);
                _logger.LogInformation("Database '{DatabaseName}' is accessible. Proceeding with migration.", databaseName);
            }
            catch (SqlException ex)
            {
                _logger.LogWarning("Could not test database connection: {Error}. Proceeding with migration anyway...", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Could not test database connection: {Error}. Proceeding with migration anyway...", ex.Message);
            }

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                _logger.LogInformation("Checking if database exists...");

                bool databaseExists = false;
                try
                {
                    databaseExists = await dbContext.Database.CanConnectAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Cannot determine if database exists: {Error}", ex.Message);
                    databaseExists = false;
                }

                if (!databaseExists)
                {
                    _logger.LogInformation("Database does not exist. Skipping migration - database should be created through infrastructure (Aspire) or manually.");
                    return;
                }

                _logger.LogInformation("Database exists. Checking for EF migrations history table...");

                bool migrationsHistoryExists = false;
                int migrationsCount = 0;

                try
                {
                    using var sqlConnection = new SqlConnection(connectionString);
                    await sqlConnection.OpenAsync(stoppingToken);

                    using var checkTableCommand = sqlConnection.CreateCommand();
                    checkTableCommand.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '__EFMigrationsHistory'";
                    var tableExistsResult = await checkTableCommand.ExecuteScalarAsync(stoppingToken);
                    migrationsHistoryExists = Convert.ToInt32(tableExistsResult) > 0;

                    _logger.LogInformation("EF Migrations History table exists: {Exists}", migrationsHistoryExists);

                    if (migrationsHistoryExists)
                    {
                        using var countCommand = sqlConnection.CreateCommand();
                        countCommand.CommandText = "SELECT COUNT(*) FROM [__EFMigrationsHistory]";
                        var countResult = await countCommand.ExecuteScalarAsync(stoppingToken);
                        migrationsCount = Convert.ToInt32(countResult);

                        _logger.LogInformation("Existing migrations count: {Count}", migrationsCount);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Cannot check EF Migrations History table: {Error}", ex.Message);
                    migrationsHistoryExists = false;
                    migrationsCount = 0;
                }

                if (!migrationsHistoryExists)
                {
                    _logger.LogInformation("EF Migrations History table does not exist. Applying all migrations to initialize the database schema...");

                    try
                    {
                        await dbContext.Database.MigrateAsync(stoppingToken);
                        _logger.LogInformation("All migrations applied successfully. Database schema initialized.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to apply migrations to initialize database schema.");
                        throw;
                    }
                    return;
                }

                if (migrationsCount == 0)
                {
                    _logger.LogInformation("EF Migrations History table exists but is empty. Inserting initial migration data...");

                    try
                    {
                        await InsertInitialCreateMigrationData(connectionString, stoppingToken);
                        _logger.LogInformation("Initial migration data inserted successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to insert initial migration data.");
                        throw;
                    }
                }

                _logger.LogInformation("Checking for pending migrations...");

                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(stoppingToken);
                if (!pendingMigrations.Any())
                {
                    _logger.LogInformation("No pending migrations found. Database is up to date.");
                    return;
                }

                _logger.LogInformation("Found {Count} pending migrations: {Migrations}",
                    pendingMigrations.Count(), string.Join(", ", pendingMigrations));

                _logger.LogInformation("Applying pending migrations...");
                await dbContext.Database.MigrateAsync(stoppingToken);
                _logger.LogInformation("Pending migrations applied successfully.");
            });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred during database migration for {DbContextName}.", typeof(TContext).Name);
            Environment.ExitCode = -1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database migration for {DbContextName}.", typeof(TContext).Name);
            Environment.ExitCode = -1; // Set error code for Aspire
        }
        finally
        {
            _logger.LogInformation("Migration service for {DbContextName} is stopping.", typeof(TContext).Name);
            _hostApplicationLifetime.StopApplication();
        }
    }

    /// <summary>
    /// Inserts initial migration data into __EFMigrationsHistory table
    /// </summary>
    private async Task InsertInitialCreateMigrationData(string connectionString, CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        var initialMigrationId = FindInitialCreateMigrationId();
        if (string.IsNullOrEmpty(initialMigrationId))
        {
            _logger.LogWarning("No InitialCreate migration found. Skipping initial migration data insertion.");
            return;
        }

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
            SELECT @migrationId, @productVersion
            WHERE NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = @migrationId);";

        command.Parameters.Add(new SqlParameter("@migrationId", initialMigrationId));
        command.Parameters.Add(new SqlParameter("@productVersion", "9.0.8"));

        await command.ExecuteNonQueryAsync(cancellationToken);
        _logger.LogInformation("Initial migration data inserted: {MigrationId}", initialMigrationId);
    }

    /// <summary>
    /// Finds the InitialCreate migration ID from the migrations assembly
    /// </summary>
    private string FindInitialCreateMigrationId()
    {
        try
        {
            var migrationsPath = Path.Combine(AppContext.BaseDirectory, "Migrations");
            if (!Directory.Exists(migrationsPath))
            {
                _logger.LogWarning("Migrations directory not found at {Path}.", migrationsPath);
                return null;
            }

            var migrationFiles = Directory.GetFiles(
                migrationsPath,
                "*_InitialCreate.cs",
                SearchOption.TopDirectoryOnly);

            if (migrationFiles.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(migrationFiles[0]);
                _logger.LogInformation("Found InitialCreate migration: {MigrationId}", fileName);
                return fileName;
            }

            _logger.LogWarning("No InitialCreate migration files found in Migrations directory.");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Could not find InitialCreate migration files: {Error}", ex.Message);
            return null;
        }
    }
} 
