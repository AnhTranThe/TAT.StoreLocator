using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                BaseResponseResult<Core.Models.Response.Category.CategoryResponseModel> response = await _categoryService.GetById(id);

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpGet("getListCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetList([FromQuery] BasePaginationRequest request)
        {
            try
            {
                BasePaginationResult<Core.Models.Response.Category.CategoryResponseModel> response = await _categoryService.GetListAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpGet("GetListParentCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListParentCategory([FromQuery] BasePaginationRequest request)
        {
            try
            {
                BasePaginationResult<Core.Models.Response.Category.CategoryResponseModel> response = await _categoryService.GetListParentCategoryAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpGet("getListSubCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListSubCategory([FromQuery] BasePaginationRequest request)
        {
            try
            {
                BasePaginationResult<Core.Models.Response.Category.CategoryResponseModel> response = await _categoryService.GetListSubCategoryAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}