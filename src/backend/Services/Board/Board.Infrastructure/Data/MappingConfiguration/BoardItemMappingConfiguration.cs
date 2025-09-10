using Board.Domain.Contracts.Enums;
using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Data.MappingConfiguration;

public class BoardItemMappingConfiguration : IEntityTypeConfiguration<BoardItem>
{
    public void Configure(EntityTypeBuilder<BoardItem> builder)
    {
        builder
            .Property(b => b.Id)
            .IsRequired();

        builder
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder
            .Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(1000000);

        builder
            .Property(b => b.Priority)
            .IsRequired();

        builder
            .Property(b => b.AssigneeId)
            .IsRequired();

        builder
            .Property(b => b.DueDate)
            .IsRequired();

        builder
            .Property(b => b.CreatedTime)
            .IsRequired();

        builder
            .Property(b => b.TaskType)
            .IsRequired()
            .HasDefaultValue(TaskTypeEnum.UserStory);

        // Explicitly ignore unconfigured self-referencing navigation to prevent unintended join tables for now
        builder.Ignore(b => b.SubItems);
    }
}

