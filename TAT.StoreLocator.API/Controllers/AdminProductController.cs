using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Product;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/admin/product")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.RoleAdminName)]
    public class AdminProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public AdminProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddProduct([FromBody] ProductRequestModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.StoreId))
                {
                    return BadRequest("StoreId is not null");
                }

                BaseResponse Response = await _productService.AddProduct(request);

                return !Response.Success ? BadRequest(Response.Message) : Ok(Response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{Id}")]
        public async Task<IActionResult> UpdateProduct(string Id, [FromBody] ProductRequestModel request)
        {
            if (request == null || string.IsNullOrEmpty(Id))
            {
                return BadRequest(new BaseResponse
                {
                    Success = false,
                    Message = "Invalid request data."
                });
            }

            BaseResponse response = await _productService.UpdateProduct(Id, request);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
    }
}