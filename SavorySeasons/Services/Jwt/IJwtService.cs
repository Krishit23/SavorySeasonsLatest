using System.Security.Claims;

namespace SavorySeasons.Services.Jwt
{
    public interface IJwtService
    {
        string CreateToken(List<Claim> claims, int duration);

        string CreateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
