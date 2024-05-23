using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Category;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/admin/category")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.RoleAdminName)]
    public class AdminCategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CategoryRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                BaseResponse response = await _categoryService.Add(request);

                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CategoryRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                BaseResponse response = await _categoryService.Update(id, request);

                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}