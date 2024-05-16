using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.RoleAdminName)]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminUserController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpGet("GetListUser")]
        public async Task<IActionResult> GetListUser()
        {
            try
            {
                BasePaginationRequest request = new();
                BasePaginationResult<UserResponseModel> UserLs = await _userService.GetListUserAsync(request);
                return Ok(UserLs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }


        [HttpPost("SearchUser")]
        public async Task<IActionResult> SearchUser(SearchUserPagingRequestModel request)
        {
            try
            {

                BasePaginationResult<UserResponseModel> UserLs = await _userService.SearchUserAsync(request);
                return Ok(UserLs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }

        }


        [HttpGet("get-user-by-id/{userId}")]
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


        [HttpGet("DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                ChangeStatusUserRequestModel request = new();
                BaseResponse result = await _userService.ChangeStatusUser(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
