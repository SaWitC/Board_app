using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Data.MappingConfiguration;

public class BoardTemplateMappingConfiguration : IEntityTypeConfiguration<BoardTemplate>
{
    public void Configure(EntityTypeBuilder<BoardTemplate> builder)
    {
        builder
            .Property(t => t.Id)
            .IsRequired();

        builder
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(10000);

        builder
            .HasOne(t => t.Board)
            .WithMany()
            .HasForeignKey(t => t.BoardId);
    }
}


