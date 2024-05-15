using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Response.Authentication;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Infrastructure.Services;

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


        [HttpPost("addProduct")]
        public async Task<ActionResult> AddProduct([FromBody] ProductRequestModel request)
        {

            try
            {
    
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest(" not found");
                }


                BaseResponse Response = await _productService.AddProduct(request);

                return !Response.Success ? BadRequest(Response.Message) : Ok(Response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }


        }

    }
}
