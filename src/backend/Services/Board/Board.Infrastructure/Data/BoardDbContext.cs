using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Board.Infrastructure.Data;

public class BoardDbContext : DbContext
{
    protected readonly string _connectionString = "Data Source=127.0.0.1,1435;Initial Catalog=BoardDb;User ID=sa;Password=u7s8BkDq8lbDS3tCRPA5;TrustServerCertificate=true";

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Domain.Entities.Board> Boards { get; set; }
    public DbSet<BoardItem> BoardItems { get; set; }
    public DbSet<BoardColumn> BoardColumns { get; set; }
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString, o =>
            {
                o.EnableRetryOnFailure();
                o.MigrationsAssembly("Board.Infrastructure");
            });
        }

        //optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Apply all mapping configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BoardDbContext).Assembly);
    }
}

