using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.API.MiddleWares
{
    public class JwtMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IJwtService _jwtService;
        public JwtMiddleWare(RequestDelegate next, IJwtService jwtService)
        {
            _next = next;
            _jwtService = jwtService;
        }
        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Headers["Authorization"]
     .FirstOrDefault(header => header.StartsWith("Bearer "))
     ?["Bearer ".Length..];

            if (token != null && _jwtService.ValidationJwtToken(token))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }

    }
}
