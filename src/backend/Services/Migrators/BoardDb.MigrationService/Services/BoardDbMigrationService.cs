using Board.Infrastructure.Data;
using Board.Infrastructure.Services;

namespace BoardDb.MigrationService.Services;

/// <summary>
/// Migration service for project database (BoardDbContext)
/// </summary>
public class BoardDbMigrationService : BaseMigrationService<BoardDbContext>
{
    /// <summary>
    /// Constructor for migration service
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    /// <param name="hostApplicationLifetime">Application lifetime</param>
    /// <param name="logger">Logger</param>
    public BoardDbMigrationService(
        IServiceProvider serviceProvider, 
        IHostApplicationLifetime hostApplicationLifetime, 
        ILogger<BoardDbMigrationService> logger) 
        : base(serviceProvider, hostApplicationLifetime, logger)
    {
    }
} 
