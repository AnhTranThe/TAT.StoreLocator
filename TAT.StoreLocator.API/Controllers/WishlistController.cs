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

        [HttpPost("product/changeWishlist/Status")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusProduct([FromBody] WishListRequestProduct request, bool Status)
        {
            try
            {
                BaseResponseResult<bool> result = await _wishlistService.ChangeStatusProduct(request, Status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("product/getWishlist")]
        [Authorize]
        public async Task<IActionResult> GetStatusProduct([FromBody] WishListRequestProduct request)
        {
            try
            {
                BaseResponseResult<bool> result = await _wishlistService.GetStatusProduct(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("store/getWhislist")]
        [Authorize]
        public async Task<IActionResult> GetStatusStore([FromBody] WishListRequestStore request)
        {
            try
            {
                BaseResponseResult<bool> result = await _wishlistService.GetStatusStore(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("store/changeWhislist/status")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusStore([FromBody] WishListRequestStore request, bool Status)
        {
            try
            {
                BaseResponseResult<bool> result = await _wishlistService.ChangeStatusStore(request, Status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




    }
}
