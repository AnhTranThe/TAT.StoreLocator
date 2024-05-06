using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Response.Authentication;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly AppDbContext _context;


        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger logger, IMapper mapper, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _context = context;


        }
        public async Task<RegisterResponseModel> RegisterUserAsync(RegisterRequestModel model)
        {


            RegisterResponseModel response = new();
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

                MailAddress address = new(model.Email ?? "");
                string userName = address.User;
                User newUser = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = model.LastName + " " + model.FirstName,
                    Email = model.Email,
                    UserName = userName,
                    AddressId = Guid.NewGuid().ToString()

                };
                Address newAddress = new()
                {
                    Id = newUser.AddressId // Use the same Id for Address as assigned to AddressId of User
                };

                _ = await _context.Addresses.AddAsync(newAddress);
                _ = await _context.SaveChangesAsync(newUser.Id);
                IdentityResult createUserResult = await _userManager.CreateAsync(newUser, model.Password?.Trim());

                if (!createUserResult.Succeeded)
                {
                    baseResponse.Message = "Error while creating user";
                    return response;
                }
                IdentityResult addRoleResult = await _userManager.AddToRoleAsync(newUser, GlobalConstants.RoleUserName);
                if (!addRoleResult.Succeeded)
                {
                    baseResponse.Message = "Error while adding role";
                    return response;
                }


                response.UserResponseModel = _mapper.Map<UserResponseModel>(newUser);
                baseResponse.Success = true;
                baseResponse.Message = "Register success";

            }
            catch (Exception ex)
            {
                baseResponse.Message = $"An error occurred while check existed the entity: {ex.Message}";
                _logger.LogError(ex);

            }

            return response;

        }

        public async Task<LoginResponseModel> LoginUserAsync(LoginRequestModel model)
        {
            LoginResponseModel response = new();
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
                    SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (!signInResult.Succeeded)
                    {
                        // Invalid password
                        baseResponse.Message = "Invalid password";
                    }

                    response.UserResponseModel = _mapper.Map<UserResponseModel>(user);
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
                    response.claims = claims;
                    baseResponse.Success = true;
                    baseResponse.Message = "Login success";

                }

            }
            catch (Exception ex)
            {
                baseResponse.Message = $"An error occurred while check existed the entity: {ex.Message}";
                _logger.LogError(ex);
            }

            return response;

        }

        public async Task<BaseResponse> LogoutUserAsync(string UserId)
        {
            BaseResponse response = new()
            {
                Success = false
            };

            try
            {
                User user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    response.Message = GlobalConstants.MessageUserNotFound;
                    return response;
                }

                await _signInManager.SignOutAsync();

                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    response.Message = "Can not update refresh token ${user.Email}";
                    return response;

                }

                response.Success = true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);

            }
            return response;


        }



    }
}
