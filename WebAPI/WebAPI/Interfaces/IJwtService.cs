using System.Security.Claims;

namespace WebAPI.Interfaces
{
    public interface IJwtService
    {
        public TokensDTO GenerateAccessAndRefreshTokens(List<Claim> claims);
    }
}