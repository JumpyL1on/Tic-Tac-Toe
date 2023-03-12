using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.EntityConfigurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable(nameof(Game));

            builder
                .Property(game => game.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .HasOne(game => game.Field)
                .WithOne(field => field.Game)
                .HasForeignKey<Field>(field => field.Id)
                .IsRequired();
        }
    }
}