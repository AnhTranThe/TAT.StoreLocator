using System.Security.Claims;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken(string email, string userName, ICollection<string>? roles, string v);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}
