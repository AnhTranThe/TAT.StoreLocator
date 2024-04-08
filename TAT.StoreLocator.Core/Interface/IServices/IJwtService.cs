using System.Security.Claims;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);






    }
}
