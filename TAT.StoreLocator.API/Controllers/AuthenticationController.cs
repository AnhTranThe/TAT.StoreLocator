using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Authentication;
using TAT.StoreLocator.Core.Utils;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {

        private readonly Core.Interface.IServices.IAuthenticationService _authenticationService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;


        public AuthenticationController(
            Core.Interface.IServices.IAuthenticationService authenticationService,
            UserManager<User> userManager,
            IUserService userService,
            IJwtService jwtService

            )
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
            _userService = userService;
            _jwtService = jwtService;


        }
        [HttpPost, Route(nameof(Register))]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (model.Email != null && await _userManager.Users.AnyAsync(u => u.NormalizedEmail == model.Email.ToUpper()))
            {
                return BadRequest("Email is already registered.");
            }

            try
            {
                RegisterResponseModel response = await _authenticationService.RegisterUserAsync(model);
                return response.BaseResponse.Success ? Ok(response) : BadRequest(response.BaseResponse.Message);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPost, Route(nameof(Login))]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                LoginResponseModel loginResponse = await _authenticationService.LoginUserAsync(request);
                if (!loginResponse.BaseResponse.Success)
                {
                    return BadRequest(loginResponse.BaseResponse.Message);
                }

                string accessToken = _jwtService.GenerateAccessToken(loginResponse.claims);

                string[] roles = loginResponse.UserResponseModel.Roles.ToArray(); // Convert roles to an array of strings

                string refreshToken = _jwtService.GenerateRefreshToken(
                    loginResponse.UserResponseModel.Email,
                    loginResponse.UserResponseModel.UserName,
                    loginResponse.UserResponseModel.Roles, // Pass roles array
                    loginResponse.UserResponseModel.Id.ToString()
                );

                UpdateJwtUserInfoRequestModel updateJwtUserInfoRequestModel = new()
                {
                    UserId = loginResponse.UserResponseModel.Id,
                    RefreshToken = refreshToken,
                };
                BaseResponse updateJwtResponse = await _userService.UpdateJwtUserInfo(updateJwtUserInfoRequestModel);
                return !updateJwtResponse.Success
                    ? BadRequest(updateJwtResponse.Message)
                    : Ok(new AuthenticationResponseModel
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken
                    });
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }
        }

        [HttpPost, Route(nameof(Logout))]
        [Authorize] // Only authenticated users can log out
        public async Task<ActionResult> Logout()
        {
            try
            {
                string id = HttpContext.User.GetClaimUserId();


                BaseResponse logoutResponse = await _authenticationService.LogoutUserAsync(id);

                return !logoutResponse.Success ? BadRequest(logoutResponse.Message) : Ok("Logout successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //[HttpGet(nameof(RefreshToken))]
        //[Authorize]

        [HttpPost, Route(nameof(Revoke))]
        [Authorize]
        public async Task<ActionResult> Revoke()
        {
            string id = HttpContext.User.GetClaimUserId();
            User user = await _userManager.Users
                .SingleAsync(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("User not found");
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            _ = await _userManager.UpdateAsync(user);
            return NoContent();
        }


    }
}
