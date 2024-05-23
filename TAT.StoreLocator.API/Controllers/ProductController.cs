using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Product;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(string productId)
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

        [HttpGet("getListProducts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProduct([FromQuery] BasePaginationRequest request)
        {
            request ??= new BasePaginationRequest();

            try
            {
                BasePaginationResult<ProductResponseModel> Products = await _productService.GetListProductAsync(request);
                return Ok(Products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }


        [HttpGet("Product/{storeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdStore(string storeId, [FromQuery] BasePaginationRequest request)
        {
            if (string.IsNullOrWhiteSpace(storeId))
            {
                return BadRequest("StoreId cannot be null or empty.");
            }
            request ??= new BasePaginationRequest();

            try
            {
                BasePaginationResult<ProductResponseModel> response = await _productService.GetByIdStore(storeId, request);

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred." + ex.Message);
            }
        }







    }
}
