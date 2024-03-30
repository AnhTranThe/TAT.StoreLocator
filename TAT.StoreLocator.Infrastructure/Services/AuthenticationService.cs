using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;


        public AuthenticationService(UserManager<User> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;

            _dbContext = appDbContext;

        }

        public async Task<ChangePasswordUserResponseModel> ChangePasswordAsync(ChangePasswordRequestModel model)
        {
            ChangePasswordUserResponseModel response = new();
            BaseResponse baseResponse = response.BaseResponse;
            baseResponse.Success = false;
            try
            {
                User user = await _userManager.FindByIdAsync(model.RequestId);

                if (user == null)
                {
                    baseResponse.Message = "User not found";
                    return response;
                }
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    baseResponse.Message = "The new password and confirmation password do not match.";
                    return response;
                }

                Task<IdentityResult> result = _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!result.Result.Succeeded)
                {
                    baseResponse.Message = "Error while changing password";
                    return response;

                }
                baseResponse.Success = true;
                baseResponse.Message = "Change password success";

            }
            catch (Exception ex)
            {
                baseResponse.Message = $"An error occurred while check existed the entity: {ex.Message}";
            }

            return response;
        }

        public async Task<RegisterUserResponseModel> RegisterUserAsync(RegisterUserRequestModel model)
        {
            RegisterUserResponseModel response = new();
            BaseResponse baseResponse = response.BaseResponse;
            baseResponse.Success = false;
            try
            {

                User existedUser = await _userManager.FindByEmailAsync(model.Email);
                if (existedUser != null)
                {
                    baseResponse.Message = "User existed";
                    return response;
                }

                if (model.Password != model.ConfirmPassword)
                {
                    baseResponse.Message = "The password and confirmation password do not match.";
                    return response;
                }


                User newUser = new()
                {

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = model.LastName + " " + model.FirstName,
                    Email = model.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password?.Trim());

                if (!result.Succeeded)
                {
                    baseResponse.Message = "Error while creating user";
                }

                //add user by default in user role
                Role? appUserRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == GlobalConstants.CustomerRoleName);
                if (appUserRole != null)
                {

                    IdentityUserRole<string> userRole = new()
                    {
                        RoleId = appUserRole.Id,
                        UserId = newUser.Id
                    };

                    _ = await _dbContext.UserRoles.AddAsync(userRole);

                }

                _ = await _dbContext.SaveChangesAsync();

                baseResponse.Success = true;
                baseResponse.Message = "Register success";

            }
            catch (Exception ex)
            {
                baseResponse.Message = $"An error occurred while check existed the entity: {ex.Message}";
            }

            return response;

        }

        public async Task<LoginUserResponseModel> ValidatePasswordAsync(LoginUserRequestModel model)
        {

            LoginUserResponseModel response = new();
            BaseResponse baseResponse = response.BaseResponse;
            baseResponse.Success = false;
            try
            {
                User user = await _userManager.FindByEmailAsync(model.EmailOrUserName) ?? await _userManager.FindByNameAsync(model.EmailOrUserName);
                if (user == null)
                {
                    // User not found
                    baseResponse.Message = "User not found";
                }
                else
                {
                    bool isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (!isValidPassword)
                    {
                        // Invalid password
                        baseResponse.Message = "Invalid password";
                    }
                    baseResponse.Success = true;
                    baseResponse.Message = "Login success";

                }

            }
            catch (Exception ex)
            {
                baseResponse.Message = $"An error occurred while check existed the entity: {ex.Message}";
            }

            return response;

        }


    }
}
