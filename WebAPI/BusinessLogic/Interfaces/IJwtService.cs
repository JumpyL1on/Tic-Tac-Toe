using Common;
using System.Security.Claims;

namespace BusinessLogic.Interfaces
{
    public interface IJwtService
    {
        public TokensDTO GenerateAccessAndRefreshTokens(List<Claim> claims);
    }
}