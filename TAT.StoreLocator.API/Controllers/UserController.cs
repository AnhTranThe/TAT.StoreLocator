using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public UserController(UserManager<User> userManager, IUserService userService, IJwtService jwtService)
        {
            _userManager = userManager;
            _userService = userService;
            _jwtService = jwtService;

        }
        [HttpGet("get-user-by-id/{userId}")]
        public async Task<IActionResult> getUserById(string userId)
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


        //[HttpPut]
        //public async Task<IActionResult> Update([FromForm] EditUserRequestModel request)
        //{
        //    EditUserResponseModel response = await _userService.UpdateUserAsync(request);
        //    if (userId == null)
        //    {
        //        return BadRequest();

        //    }

        //    return Ok(new { message = "Cập nhập user thành công!", user });
        //}






        //[HttpPost("ForgetPassword")]
        //public async Task<IActionResult> ForgetPassword(ForgotPasswordRequestModel request)
        //{
        //    var check = await _userService.ResetPasswordAsync(request);
        //    return Ok(check);
        //}

    }
}
