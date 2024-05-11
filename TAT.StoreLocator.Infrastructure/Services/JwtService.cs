using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class JwtService : IJwtService
    {

        private readonly JwtTokenSettings _jwtTokenSettings;
        protected IHttpContextAccessor _httpContextAccessor;

        public JwtService(IOptions<JwtTokenSettings> jwtTokenSettings, IHttpContextAccessor httpContextAccessor)

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
                expires: DateTime.Now.AddHours(_jwtTokenSettings.ExpireInHours),
                signingCredentials: signinCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken(string email, string userName, ICollection<string>? roles, string userId)
        {
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName),
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
    }


}

