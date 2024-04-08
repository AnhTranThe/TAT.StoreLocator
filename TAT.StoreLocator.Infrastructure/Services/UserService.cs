using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Role;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(UserManager<User> userManager,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ILogger logger,
        IHttpContextAccessor httpContextAccessor
        )

        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequestModel request)
        {
            BaseResponse response = new()
            {
                Success = false
            };
            try
            {
                User user = await _userManager.FindByIdAsync(request.RequestId);

                if (user == null)
                {
                    response.Message = "User not found";
                    return response;
                }
                if (request.NewPassword != request.ConfirmNewPassword)
                {
                    response.Message = "The new password and confirmation password do not match.";
                    return response;
                }

                Task<IdentityResult> result = _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!result.Result.Succeeded)
                {
                    response.Message = "Error while changing password";
                    return response;

                }
                response.Success = true;
                response.Message = "Change password success";

            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred while check existed the entity: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse> Delete(string id)
        {
            BaseResponse response = new()
            {
                Success = false
            };

            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.Message = "User not found.";
                return response;
            }
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                response.Message = "Failed to delete {user.Email}";
                return response;
            }

            response.Success = true;
            return response;


        }

        public async Task<BaseResponseResult<UserResponseModel>> GetById(string id)
        {
            BaseResponseResult<UserResponseModel> response = new()
            {
                Success = false
            };
            try
            {
                User user = await _userManager.FindByIdAsync(id);
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

        public async Task<BasePaginationResult<UserResponseModel>> GetUsersPaging(GetUserPagingRequestModel request)
        {
            BasePaginationResult<UserResponseModel> response = new();

            IQueryable<User> query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.keyword));
            }

            int totalRow = await query.CountAsync();

            List<UserResponseModel> data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new UserResponseModel()
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    Email = p.Email,
                }).ToListAsync();

            // in ra

            response.TotalCount = totalRow;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Data = data;

            return response;


        }

        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequestModel request)
        {
            BaseResponse response = new()
            {
                Success = false
            };

            User user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.Message = GlobalConstants.MessageUserNotFound;
                return response;
            }
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                response.Message = "Password and confirm new password do not match.";
                return response;
            }
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.Message = "Failed to reset password ${user.Email}";
                return response;
            }

            response.Success = true;
            response.Message = "reset password successfully ${user.Email}";
            return response;
        }


        public async Task<AssignRoleResponseModel> RoleAssign(AssignRoleRequestModel request)
        {
            AssignRoleResponseModel response = new();
            BaseResponse baseResponse = new()
            {
                Success = false
            };


            User user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                baseResponse.Message = GlobalConstants.MessageUserNotFound;
                response.BaseResponse = baseResponse;
                return response;
            }
            List<string?> removedRoles = request.Roles.Where(x => !x.Selected).Select(x => x.Name).ToList();
            foreach (string? roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    _ = await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            _ = await _userManager.RemoveFromRolesAsync(user, removedRoles);

            List<string?> addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (string? roleName in addedRoles)
            {
                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    _ = await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            baseResponse.Success = true;
            baseResponse.Message = "Role assign to User succesfully";
            response.BaseResponse = baseResponse;

            return response;
        }

        public async Task<BaseResponse> UpdateJwtUserInfo(UpdateJwtUserInfoRequestModel request)
        {

            BaseResponse response = new()
            {
                Success = false
            };

            User user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                response.Message = GlobalConstants.MessageUserNotFound;
                return response;
            }

            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                response.Message = "Token not found";
                return response;
            }

            user.RefreshToken = request.RefreshToken;
            user.RefreshTokenExpiryTime = DateTimeOffset.UtcNow.AddDays(1);
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                response.Message = "Can not update refresh token ${user.Email}";
                return response;

            }

            response.Success = true;
            return response;

        }

    }
}
