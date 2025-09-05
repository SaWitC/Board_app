using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Board.Domain.Entities;

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
			.HasMaxLength(200);

		builder
			.Property(b => b.Description)
			.IsRequired()
			.HasMaxLength(10000);

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

		// Explicitly ignore unconfigured self-referencing navigation to prevent unintended join tables for now
		builder.Ignore(b => b.SubItems);
	}
}

