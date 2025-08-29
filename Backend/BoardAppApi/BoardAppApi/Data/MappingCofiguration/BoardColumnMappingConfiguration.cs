using BoardAppApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BoardAppApi.Data.MappingCofiguration
{
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
        }
    }
 
}
