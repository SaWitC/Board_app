using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Board.Domain.Entities;

namespace Board.Infrastructure.Data.MappingConfiguration;

public class TagMappingConfiguration : IEntityTypeConfiguration<Tag>
{
	public void Configure(EntityTypeBuilder<Tag> builder)
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
			.HasMaxLength(10000)
			.IsRequired(false);
	}
}

