using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic
{
    public static class UserManagerExtensions
    {
        public static async Task UpdateRefreshTokenInfoAsync(
            this UserManager<Player> userManager,
            Player player,
            string refreshToken,
            IConfiguration configuration)
        {
            player.RefreshToken = refreshToken;

            var days = int.Parse(configuration["Jwt:RefreshTokenExpiresInDays"]!);

            player.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(days);

            await userManager.UpdateAsync(player);
        }

        public static async Task<Player?> FindByRefreshTokenAsync(this UserManager<Player> userManager, string refreshToken)
        {
            return await userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
        }
    }
}