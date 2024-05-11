using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TAT.StoreLocator.Core.Helpers;

namespace TAT.StoreLocator.Infrastructure.Management
{
    public abstract class BaseService
    {
        protected IHttpContextAccessor _httpContextAccessor;
        protected BaseService(
        IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected string GetGuidUserIdLogin()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            string hello = user.FindFirstValue(UserClaims.Id);
            return hello;
        }

    }
}
