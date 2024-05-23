using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _dbContext;

        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        protected IHttpContextAccessor _httpContextAccessor;

        public ProfileService(UserManager<User> userManager,
         ILogger logger,
         AppDbContext dbContext,

        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext;

            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetGuidUserIdLogin()
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            string hello = user.FindFirstValue(UserClaims.Id);
            return hello;
        }

        public async Task<BaseResponseResult<UserResponseModel>> GetByProfile()
        {
            BaseResponseResult<UserResponseModel> response = new()
            {
                Success = false
            };
            string loginId = GetGuidUserIdLogin();
            try
            {
                User user = await _userManager.FindByIdAsync(loginId.ToString());
                if (user == null)
                {
                    response.Success = false;
                    response.Message = GlobalConstants.MessageUserNotFound;
                    return response;
                }

                IList<string> roles = await _userManager.GetRolesAsync(user);

                UserResponseModel userResponse = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList() // Convert roles to a list
                };

                response.Data = userResponse;
                response.Success = true;
                response.Message = "User found";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                // Log the exception if needed
            }

            return response;
        }
    }
}