using System.Security.Claims;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Token.Request;
using TAT.StoreLocator.Core.Models.Token.Response;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken(string email, string userName, ICollection<string>? roles, string v);
        Task<BaseResponseResult<NewToken>> RefreshToken(RefreshTokenRequest tokenModel);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);


    }
}
