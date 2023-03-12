using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable(nameof(Player));

            builder
                .HasMany(playerA => playerA.PlayerAGames)
                .WithOne(game => game.PlayerA)
                .HasForeignKey(game => game.PlayerAId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .IsRequired();

            builder
                .HasMany(playerB => playerB.PlayerBGames)
                .WithOne(game => game.PlayerB)
                .HasForeignKey(game => game.PlayerBId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .IsRequired(false);
        }
    }
}