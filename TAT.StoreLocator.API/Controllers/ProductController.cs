using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Product;
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
        public async Task<IActionResult> GetListProduct([FromQuery]BasePaginationRequest request)
        {   
            if(request == null)
            {
                request = new BasePaginationRequest();

            }
            
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

        [HttpPost("addProduct")]
        [AllowAnonymous]
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

        [HttpPut("update/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProduct(string Id ,[FromBody] ProductRequestModel request)
        {
            if (request == null || string.IsNullOrEmpty(Id))
            {
                return BadRequest(new BaseResponse
                {
                    Success = false,
                    Message = "Invalid request data."
                });
            }

            var response = await _productService.UpdateProduct(Id,request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpGet("Product/{storeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdStore(string storeId, [FromQuery] BasePaginationRequest request)
        {
            if (string.IsNullOrWhiteSpace(storeId))
            {
                return BadRequest("StoreId cannot be null or empty.");
            }
            if (request == null)
            {
                request = new BasePaginationRequest();

            }

            try
            {
                var response = await _productService.GetByIdStore(storeId, request);
                
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
