using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TAT.StoreLocator.Core.Helpers;

namespace TAT.StoreLocator.API.MiddleWares
{
    public class JwtMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly JwtTokenSettings _jwtTokenSettings;

        public JwtMiddleWare(RequestDelegate next, IOptions<JwtTokenSettings> jwtTokenSettings)
        {
            _next = next;
            _jwtTokenSettings = jwtTokenSettings.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            Endpoint? endpoint = context.GetEndpoint();
            bool allowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }
            string? token = context.Request.Headers["Authorization"]
     .FirstOrDefault(header => header.StartsWith("Bearer "))
     ?["Bearer ".Length..];

            if (token != null && ValidationJwtToken(token))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
        private bool ValidationJwtToken(string jwt)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                TokenValidationParameters validationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtTokenSettings.Issuer,
                    ValidAudience = _jwtTokenSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.Key)),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero // Adjust as needed
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);
                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    // Token has expired
                    return false;
                }

                return principal != null;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
