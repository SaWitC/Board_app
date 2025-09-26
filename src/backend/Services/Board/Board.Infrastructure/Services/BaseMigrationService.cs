using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Board.Infrastructure.Services;

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

			var connectionString = dbContext.Database.GetConnectionString();
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				_logger.LogError("Connection string is not configured for {DbContextName}.", typeof(TContext).Name);
				Environment.ExitCode = -1;
				return;
			}

			_logger.LogInformation("Starting migration for provider: {Provider}", dbContext.Database.ProviderName);

			var strategy = dbContext.Database.CreateExecutionStrategy();

			await strategy.ExecuteAsync(async () =>
			{
				try
				{
					_logger.LogInformation("Checking if database exists...");
					bool databaseExists = await dbContext.Database.CanConnectAsync(stoppingToken);

					if (!databaseExists)
					{
						_logger.LogInformation("Database does not exist. Skipping migration.");
						return;
					}

					_logger.LogInformation("Database exists. Checking for EF migrations history table...");

					bool migrationsHistoryExists = false;
					int migrationsCount = 0;

					var connection = dbContext.Database.GetDbConnection();
					var openedHere = false;
					if (connection.State != ConnectionState.Open)
					{
						await dbContext.Database.OpenConnectionAsync(stoppingToken);
						openedHere = true;
					}

					using var checkTableCommand = connection.CreateCommand();
					checkTableCommand.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_name = '__EFMigrationsHistory'";
					migrationsHistoryExists = Convert.ToInt32(await checkTableCommand.ExecuteScalarAsync(stoppingToken)) > 0;

					_logger.LogInformation("EF Migrations History table exists: {Exists}", migrationsHistoryExists);

					if (migrationsHistoryExists)
					{
						using var countCommand = connection.CreateCommand();
						countCommand.CommandText = "SELECT COUNT(*) FROM \"__EFMigrationsHistory\"";
						migrationsCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync(stoppingToken));
						_logger.LogInformation("Existing migrations count: {Count}", migrationsCount);
					}

					if (!migrationsHistoryExists)
					{
						_logger.LogInformation("EF Migrations History table does not exist. Creating it...");
						await EnsureMigrationsHistoryTableAsync(connection, stoppingToken);
						_logger.LogInformation("EF Migrations History table created. Applying all migrations...");
						await dbContext.Database.MigrateAsync(stoppingToken);
						_logger.LogInformation("All migrations applied successfully.");
					}
					else if (migrationsCount == 0)
					{
						_logger.LogInformation("EF Migrations History table is empty. Inserting initial migration data...");
						await InsertInitialCreateMigrationData(connection, null, stoppingToken);
						_logger.LogInformation("Initial migration data inserted successfully.");
					}

					var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(stoppingToken);
					if (pendingMigrations.Any())
					{
						_logger.LogInformation("Found {Count} pending migrations: {Migrations}",
							pendingMigrations.Count(), string.Join(", ", pendingMigrations));

						_logger.LogInformation("Applying pending migrations...");
						await dbContext.Database.MigrateAsync(stoppingToken);
						_logger.LogInformation("Migrations applied successfully.");
					}
					else
					{
						_logger.LogInformation("No pending migrations found. Database is up to date.");
					}

					if (openedHere)
					{
						await dbContext.Database.CloseConnectionAsync();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "An error occurred during migration.");
					throw; 
				}
			});
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred during database migration for {DbContextName}.", typeof(TContext).Name);
			Environment.ExitCode = -1;
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
	/// 
	private async Task InsertInitialCreateMigrationData(
		DbConnection connection,
		DbTransaction transaction,
		CancellationToken cancellationToken)
	{
		var initialMigrationId = FindInitialCreateMigrationId();
		if (string.IsNullOrEmpty(initialMigrationId))
		{
			_logger.LogWarning("No InitialCreate migration found. Skipping initial migration data insertion.");
			return;
		}

		using var command = connection.CreateCommand();
		if (transaction != null)
		{
			command.Transaction = transaction; 
		}
		command.CommandText = @"
		INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
		SELECT @migrationId, @productVersion
		WHERE NOT EXISTS (SELECT 1 FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = @migrationId);";

		var migrationIdParam = command.CreateParameter();
		migrationIdParam.ParameterName = "@migrationId";
		migrationIdParam.Value = initialMigrationId;
		command.Parameters.Add(migrationIdParam);

		var productVersionParam = command.CreateParameter();
		productVersionParam.ParameterName = "@productVersion";
		productVersionParam.Value = "9.0.8";
		command.Parameters.Add(productVersionParam);

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
			var migrationsAssembly = _serviceProvider.GetService<IMigrationsAssembly>();
			if (migrationsAssembly == null)
			{
				_logger.LogWarning("IMigrationsAssembly service not found.");
				return null;
			}

			// Migration keys are like: 20250904164558_InitialCreate
			var initial = migrationsAssembly.Migrations.Keys
				.FirstOrDefault(k => k.EndsWith("_InitialCreate", StringComparison.OrdinalIgnoreCase));
			if (!string.IsNullOrEmpty(initial))
			{
				_logger.LogInformation("Found InitialCreate migration: {MigrationId}", initial);
				return initial;
			}

			_logger.LogWarning("No InitialCreate migration found in assembly metadata.");
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogWarning("Could not resolve InitialCreate migration from assembly: {Error}", ex.Message);
			return null;
		}
	}

	private static async Task EnsureMigrationsHistoryTableAsync(DbConnection connection, CancellationToken cancellationToken)
	{
		var openedHere = false;
		if (connection.State != ConnectionState.Open)
		{
			await connection.OpenAsync(cancellationToken);
			openedHere = true;
		}

		await using var cmd = connection.CreateCommand();
		cmd.CommandText = @"
		CREATE TABLE IF NOT EXISTS public.""__EFMigrationsHistory"" (
			""MigrationId"" character varying(150) NOT NULL,
			""ProductVersion"" character varying(32) NOT NULL,
			CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
		);";
		await cmd.ExecuteNonQueryAsync(cancellationToken);

		if (openedHere)
		{
			await connection.CloseAsync();
		}
	}
} 
