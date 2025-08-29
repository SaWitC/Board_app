using BoardAppApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace BoardAppApi.Data
{
    public class BoardAppDbContext : DbContext
    {
        DbSet<Tag> Tags { get; set; }
        DbSet<Board> Boards { get; set; }
        DbSet<BoardItem> BoardItems { get; set; }
        DbSet<BoardColumn> BoardColumns { get; set; }

        public BoardAppDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {
            optionsBuilder.UseSqlServer("Data Source=host.docker.internal,1435;Initial Catalog=BoardApp.Application;Persist Security Info=False;User ID=sa;Password=u7s8BkDq8lbDS3tCRPA5;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Apply all mapping configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BoardAppDbContext).Assembly);
        }
    }
}
