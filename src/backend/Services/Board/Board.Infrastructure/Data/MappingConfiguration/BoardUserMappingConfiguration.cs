using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Data.MappingConfiguration;

public class BoardUserMappingConfiguration : IEntityTypeConfiguration<BoardUser>
{
    public void Configure(EntityTypeBuilder<BoardUser> builder)
    {
        builder
            .Property(b => b.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(b => b.Role)
            .IsRequired();

        // Define composite key to uniquely identify a board participant (email is globally unique)
        builder.HasKey(b => new { b.BoardId, b.Email });

        // Useful indexes for query patterns
        builder.HasIndex(b => b.Email).HasDatabaseName("IX_BoardUsers_Email");
        builder.HasIndex(b => b.BoardId).HasDatabaseName("IX_BoardUsers_BoardId");

        // Configure relationship with Board and cascade delete of participants when board is removed
        builder
            .HasOne(x => x.Board)
            .WithMany(b => b.BoardUsers)
            .HasForeignKey(b => b.BoardId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
