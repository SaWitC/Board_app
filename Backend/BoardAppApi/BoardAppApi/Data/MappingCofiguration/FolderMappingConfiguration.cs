using BoardAppApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoardAppApi.Data.MappingCofiguration;

public class FolderMappingConfiguration : IEntityTypeConfiguration<BoardColumn>
{
    public void Configure(EntityTypeBuilder<BoardColumn> builder)
    {
        builder
            .Property(b => b.Id)
            .IsRequired();
    }
}
