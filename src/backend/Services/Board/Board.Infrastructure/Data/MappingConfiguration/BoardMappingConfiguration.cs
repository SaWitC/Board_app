using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Data.MappingConfiguration;

public class BoardMappingConfiguration : IEntityTypeConfiguration<Domain.Entities.Board>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Board> builder)
    {
        builder
            .Property(b => b.Id)
            .IsRequired();

        builder
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(100000);

        builder
            .HasMany(b => b.BoardColumns)
            .WithOne(x => x.Board)
            .HasForeignKey(x => x.BoardId);

        builder
            .HasMany(b => b.BoardColumns)
            .WithOne(b => b.Board)
            .HasForeignKey("BoardId");
    }
}
