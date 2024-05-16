using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TAT.StoreLocator.Core.Common;
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

        public JwtService(IOptions<JwtTokenSettings> jwtTokenSettings,

            IHttpContextAccessor httpContextAccessor)

        {
            _jwtTokenSettings = jwtTokenSettings.Value;

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
                expires: DateTime.Now.AddMinutes(_jwtTokenSettings.ExpireInMinutes),
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

            byte[] key = new byte[32]; // 256 bits
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            SymmetricSecurityKey securityKey = new(key); // Replace "your_secret_key" with your actual secret key
            SigningCredentials creds = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtTokenSettings.ExpireInMinutes), // Token expiration time, adjust as needed
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public BaseResponseResult<NewToken> RefreshToken(RefreshTokenRequest tokenModel)
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

            ClaimsPrincipal? principal = GetPrincipalFromExpiredToken(accessToken ?? "");
            if (principal == null)
            {
                result.Message = "Invalid access token or refresh token";
                return result;
            }
            // Get the expiration claim from the token
            Claim? expirationClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expirationClaim == null || !long.TryParse(expirationClaim.Value, out long expirationEpoch))
            {
                result.Message = "Expiration claim not found or invalid";
                return result;
            }
            // Convert the expiration time from epoch time to DateTime
            DateTime expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationEpoch).UtcDateTime;

            // Compare the expiration time with the current time
            if (expirationDateTime <= DateTime.UtcNow)
            {
                result.Message = "Access token has expired";
                return result;
            }

            string newAccessToken = GenerateAccessToken(principal.Claims.ToList());
            result.Success = true;
            result.Message = "Refresh token successfully";
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
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };


            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            return securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
                ? throw new SecurityTokenException("Invalid token")
                : principal;
        }


        //public ClaimsPrincipal? GetPrincipalFromExpiredTokenv2(string token)
        //{
        //    TokenValidationParameters tokenValidationParameters = new()
        //    {
        //        ValidateAudience = false,
        //        ValidateIssuer = false,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.Key)),
        //        ValidateLifetime = false
        //    };

        //    JwtSecurityTokenHandler tokenHandler = new();
        //    ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        //    return securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
        //        ? throw new SecurityTokenException("Invalid token")
        //        : principal;
        //}


    }


}

