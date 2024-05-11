using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {


        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;

        }
        [HttpGet("get/{userId}")]
        [Authorize(Roles = GlobalConstants.RoleUserName)]
        public async Task<IActionResult> GetUserById(string userId)
        {


            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("User not found");
                }

                BaseResponseResult<UserResponseModel> userResponse = await _userService.GetById(userId);

                return !userResponse.Success ? BadRequest(userResponse.Message) : Ok(userResponse);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }
        }
        [HttpPut("update/user/{userId}")]
        [Authorize(Roles = GlobalConstants.RoleUserName)]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequestModel request)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(request.RequestId))
                {
                    return BadRequest("User not found");
                }


                UpdateUserResponseModel Response = await _userService.UpdateUserAsync(request);

                return !Response.BaseResponse.Success ? BadRequest(Response.BaseResponse.Message) : Ok(Response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }

        }
        [HttpPost("ResetPassword")]
        [Authorize(Roles = GlobalConstants.RoleUserName)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel request)
        {
            BaseResponse check = await _userService.ResetPasswordAsync(request);
            return Ok(check);
        }
        [HttpPost("ChangePassword")]
        [Authorize(Roles = GlobalConstants.RoleUserName)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel request)
        {
            BaseResponse check = await _userService.ChangePasswordAsync(request);
            return Ok(check);
        }


    }
}
