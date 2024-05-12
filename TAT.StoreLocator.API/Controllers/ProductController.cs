using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Product;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        [HttpGet("get/{productId}")]

        public async Task<IActionResult> GetProductById([FromQuery] string productId)
        {


            try
            {
                if (string.IsNullOrWhiteSpace(productId))
                {
                    return BadRequest("Product not found");
                }

                BaseResponseResult<ProductResponseModel> productReponse = await _productService.GetById(productId);

                return !productReponse.Success ? BadRequest(productReponse.Message) : Ok(productReponse);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }
        }
        [HttpGet("GetListProducts")]
        public async Task<IActionResult> GetListUser()
        {
            try
            {
                BasePaginationRequest request = new();
                BasePaginationResult<ProductResponseModel> Products = await _productService.GetListProductAsync(request);
                return Ok(Products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }

    }
}
