using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations
{
    public class FieldConfiguration : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.ToTable(nameof(Game));

            builder
                .Property(game => game.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}