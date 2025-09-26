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
            .Property(b => b.AssigneeEmail)
            .IsRequired(false);

        builder
            .Property(b => b.DueDate)
            .IsRequired(false);

        builder
            .Property(b => b.CreatedTime)
            .IsRequired();

        builder
            .Property(b => b.TaskType)
            .IsRequired()
            .HasDefaultValue(TaskTypeEnum.UserStory)
            .HasSentinel(TaskTypeEnum.UserStory);

        builder
            .HasOne(x => x.Assignee)
            .WithMany(b => b.Items)
            .HasForeignKey(b => new { b.BoardId, b.AssigneeEmail })
            .IsRequired(false);

        builder
            .HasMany(b => b.Tags)
            .WithMany(t => t.BoardItems);

        // Explicitly ignore unconfigured self-referencing navigation to prevent unintended join tables for now
        builder.Ignore(b => b.SubItems);
    }
}

