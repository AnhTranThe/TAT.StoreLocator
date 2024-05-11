using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Token.Request;
using TAT.StoreLocator.Core.Models.Token.Response;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class JwtService : IJwtService
    {

        private readonly JwtTokenSettings _jwtTokenSettings;
        protected IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public JwtService(IOptions<JwtTokenSettings> jwtTokenSettings,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)

        {
            _jwtTokenSettings = jwtTokenSettings.Value;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey secretKey = new(System.Text.Encoding.UTF8.GetBytes(_jwtTokenSettings.Key));
            SigningCredentials signinCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokeOptions = new(
                issuer: _jwtTokenSettings.Issuer,
                audience: _jwtTokenSettings.Issuer,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken(string email, string userName, ICollection<string>? roles, string userId)
        {
            Claim[] claims = new[]
            {
        new Claim(UserClaims.Email, email),
        new Claim(UserClaims.UserName, userName),
        new Claim(UserClaims.Id, userId),
    };

            if (roles != null && roles.Any())
            {
                claims = claims.Concat(roles.Select(role => new Claim(ClaimTypes.Role, role))).ToArray();
            }

            var key = new byte[32]; // 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            var securityKey = new SymmetricSecurityKey(key); // Replace "your_secret_key" with your actual secret key
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3), // Token expiration time, adjust as needed
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<BaseResponseResult<NewToken>> RefreshToken(RefreshTokenRequest tokenModel)
        {
            BaseResponseResult<NewToken> result = new()
            {
                Success = false,
                Message = string.Empty,
                Data = null,
            };
            if (tokenModel is null)
            {
                result.Message = "Invalid client request";
                return result;
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredTokenv2(accessToken);
            if (principal == null)
            {
                result.Message = "Invalid access token or refresh token";
                return result;
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                result.Message = "Invalid access token or refresh token";
                return result;
            }

            var newAccessToken = GenerateAccessToken(principal.Claims.ToList());
            // var newRefreshToken = GenerateRefreshToken();

            //user.RefreshToken = newRefreshToken;
            //await _userManager.UpdateAsync(user);

            result.Data = new NewToken()
            {
                NewAccessToken = newAccessToken,
                RefreshToken = refreshToken,
            };
            return result;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtTokenSettings.Key)),
                ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
            };


            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            return securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
                ? throw new SecurityTokenException("Invalid token")
                : principal;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredTokenv2(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.Key)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }


}

