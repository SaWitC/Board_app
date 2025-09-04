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
			.HasMaxLength(10000);

		builder
			.HasMany(b => b.BoardColumns)
			.WithOne()
			.HasForeignKey("BoardId");
	}
}
