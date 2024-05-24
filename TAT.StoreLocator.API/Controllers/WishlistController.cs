using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.WishList;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("product/changestatus")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus([FromBody] WishListRequestProduct request, bool Status)
        {
            try
            {
                BaseResponseResult<bool> result = await _wishlistService.ChangeStatus(request, Status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("product/getstatus")]
        [Authorize]
        public async Task<IActionResult> GetStatus([FromBody] WishListRequestProduct request)
        {
            try
            {
                BaseResponseResult<bool> result = await _wishlistService.GetStatus(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
