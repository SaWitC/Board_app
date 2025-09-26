using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data;

public class BoardDbContext : DbContext
{
    public DbSet<Domain.Entities.Board> Boards { get; set; }
    public DbSet<BoardItem> BoardItems { get; set; }
    public DbSet<BoardColumn> BoardColumns { get; set; }
    public DbSet<BoardTemplate> BoardTemplates { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Apply all mapping configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BoardDbContext).Assembly);
    }
}

