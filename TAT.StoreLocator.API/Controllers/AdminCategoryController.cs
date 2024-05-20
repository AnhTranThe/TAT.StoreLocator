using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Category;
using System;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Helpers;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.RoleAdminName)]
    public class AdminCategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> Add([FromBody] CategoryRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _categoryService.Add(request);

                if (response.Success)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var response = await _categoryService.GetById(id);

                if (response.Success)
                    return Ok(response);

                return NotFound(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpGet("getListCategory")]
        public async Task<IActionResult> GetList([FromQuery] BasePaginationRequest request)
        {
            try
            {
                var response = await _categoryService.GetListAsync(request);
                return Ok(response);
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
                    return BadRequest(ModelState);

                var response = await _categoryService.Update(id, request);

                if (response.Success)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
