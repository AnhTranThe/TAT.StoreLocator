using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager)

        {
            _jwtTokenSettings = jwtTokenSettings.Value;

            _httpContextAccessor = httpContextAccessor;

            _userManager = userManager;
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

        public async Task<string> GenerateAccessTokenV2(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ArgumentException(GlobalConstants.USER_NOT_FOUND);
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            Claim[] claims = new[]
         {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(UserClaims.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(UserClaims.FirstName, user.FirstName??""),
                    new Claim(UserClaims.Roles, string.Join(";", roles)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //byte[] key = new byte[32]; // 256 bits
            //using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(key);
            //}
            SymmetricSecurityKey secretKey = new(System.Text.Encoding.UTF8.GetBytes(_jwtTokenSettings.Key));
            SigningCredentials creds = new(secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
            issuer: _jwtTokenSettings.Issuer,
            audience: _jwtTokenSettings.Issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtTokenSettings.ExpireInMinutes),
            signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenV2(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ArgumentException(GlobalConstants.USER_NOT_FOUND);
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            Claim[] claims = new[]
         {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(UserClaims.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(UserClaims.FirstName, user.FirstName??""),
                    new Claim(UserClaims.Roles, string.Join(";", roles)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("refresh", "true")
            };

            //byte[] key = new byte[32]; // 256 bits
            //using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(key);
            //}
            //  SymmetricSecurityKey securityKey = new(key);
            SymmetricSecurityKey secretKey = new(System.Text.Encoding.UTF8.GetBytes(_jwtTokenSettings.Key));

            SigningCredentials creds = new(secretKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _jwtTokenSettings.Issuer,
                audience: _jwtTokenSettings.Issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtTokenSettings.ExpireInMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<BaseResponseResult<NewToken>> RefreshToken(RefreshTokenRequest tokenRequest)
        {
            BaseResponseResult<NewToken> result = new()
            {
                Success = false,
                Message = string.Empty,
                Data = null,
            };

            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            string? username = principal.Identity?.Name;

            if (string.IsNullOrWhiteSpace(username))
            {
                result.Message = GlobalConstants.USERNAME_NOT_FOUND;
                return result;
            }
            User user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                result.Message = GlobalConstants.USER_NOT_FOUND;
                return result;
            }
            string newAccessToken = await GenerateAccessTokenV2(username);
            string newRefreshToken = await GenerateRefreshTokenV2(username);
            result.Success = true;
            result.Message = "Refresh token successfully";
            result.Data = new NewToken()
            {
                NewAccessToken = newAccessToken,
                NewRefreshToken = newRefreshToken,
            };
            return result;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
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