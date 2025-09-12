using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Data.MappingConfiguration;

public class BoardTemplateMappingConfiguration : IEntityTypeConfiguration<BoardTemplate>
{
    public void Configure(EntityTypeBuilder<BoardTemplate> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder
            .HasOne(t => t.Board)
            .WithOne(b => b.BoardTemplate)
            .HasForeignKey<BoardTemplate>(t => t.Id);

        builder
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(10000);
    }
}


