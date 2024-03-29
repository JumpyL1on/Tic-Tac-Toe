﻿using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class Player : IdentityUser<int>
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<Game>? PlayerAGames { get; }
        public ICollection<Game>? PlayerBGames { get; }
    }
}