using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Board.Domain.Entities;

namespace Board.Infrastructure.Data.MappingConfiguration;

public class BoardColumnMappingConfiguration : IEntityTypeConfiguration<BoardColumn>
{
	public void Configure(EntityTypeBuilder<BoardColumn> builder)
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
			.HasMany(b => b.Items)
			.WithOne(i => i.BoardColumn)
			.HasForeignKey(i => i.BoardColumnId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}

