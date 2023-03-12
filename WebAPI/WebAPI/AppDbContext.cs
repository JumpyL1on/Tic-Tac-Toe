using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.EntityConfigurations;

namespace WebAPI
{
    public class AppDbContext : IdentityUserContext<Player, int>
    {
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Field> Fields { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = "Server=(localdb)\\mssqllocaldb;Database=tic-tac-toe;Trusted_Connection=True;";

            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(PlayerConfiguration).Assembly);
        }
    }
}