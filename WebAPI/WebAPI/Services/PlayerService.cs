using Habr.Common.Exceptions;
using Habr.Common.Requests;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebAPI.Entities;
using WebAPI.Extensions;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly UserManager<Player> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public PlayerService(UserManager<Player> userManager, IJwtService jwtService, IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task SignUpPlayerAsync(SignUpPlayerRequest request)
        {
            var player = new Player
            {
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(player, request.Password);

            if (!result.Succeeded)
            {
                throw new BusinessException("Email is already taken");
            }
        }

        public async Task<TokensDTO> SignInPlayerAsync(SignInPlayerRequest request)
        {
            var player = await _userManager.FindByEmailAsync(request.Email);

            if (!await _userManager.CheckPasswordAsync(player, request.Password))
            {
                throw new BusinessException("Wrong email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, player.Id.ToString()),
                new Claim(ClaimTypes.Email, player.Email)
            };

            var tokens = _jwtService.GenerateAccessAndRefreshTokens(claims);

            await _userManager.UpdateRefreshTokenInfoAsync(player, tokens.RefreshToken, _configuration);

            return tokens;
        }

        public async Task<TokensDTO> RefreshAccessTokenAsync(RefreshAccessTokenRequest request)
        {
            var player = await _userManager.FindByRefreshTokenAsync(request.RefreshToken);

            if (player == null || player.RefreshToken != request.RefreshToken)
            {
                throw new BusinessException("Wrong refresh token");
            }

            if (player.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new BusinessException("Refresh token is expired");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, player.Id.ToString()),
                new Claim(ClaimTypes.Email, player.Email)
            };

            var tokens = _jwtService.GenerateAccessAndRefreshTokens(claims);

            await _userManager.UpdateRefreshTokenInfoAsync(player, tokens.RefreshToken, _configuration);

            return tokens;
        }
    }
}