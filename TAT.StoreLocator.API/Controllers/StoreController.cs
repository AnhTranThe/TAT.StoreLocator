using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        //Depency Injection
        private readonly IStoreService _storeService;

        //Constructor
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreRequestModel request)
        {
            try
            {
                Core.Models.Response.Store.CreateStoreResponseModel response = await _storeService.CreateStoreAsync(request);
                return response == null
                    ? StatusCode(500, "Failed to create store")
                    : (IActionResult)CreatedAtAction(nameof(GetAllStore), new { storeid = response.Id }, response.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllStore([FromQuery] BasePaginationRequest paginationRequest)
        {
            try
            {

                var response = await _storeService.GetAllStoreAsync(paginationRequest);
                return response != null && response.Success ? Ok(response.Data) : (IActionResult)StatusCode(500, "Failed to get stores");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getDetail/{storeId}")]
        public async Task<IActionResult> GetDetailStore(string storeId)
        {
            try
            {
                GetDetailStoreRequestModel request = new() { Id = storeId };
                Core.Common.BaseResponseResult<Core.Models.Response.Store.StoreResponseModel> response = await _storeService.GetDetailStoreAsync(storeId);
                return response.Success ? Ok(response.Data) : NotFound(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{storeId}")]
        public async Task<IActionResult> UpdateStore(string storeId, [FromBody] UpdateStoreRequestModel request)
        {
            try
            {
                Core.Common.BaseResponseResult<Core.Models.Response.Store.StoreResponseModel> response = await _storeService.UpdateStoreAsync(storeId, request);
                return response.Success ? Ok(response.Data) : (IActionResult)StatusCode(500, response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete/{storeId}")]
        public async Task<IActionResult> DeleteStore(string storeId)
        {
            try
            {
                Core.Common.BaseResponse response = await _storeService.DeleteStoreAsync(storeId);
                return response.Success ? Ok("Store deleted successfully") : (IActionResult)StatusCode(500, response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("get/nearStore")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNearStore([FromQuery] GetNearStore getNearStore)
        {
            try
            {
                var response = await _storeService.GetTheNearestStore(getNearStore.District, getNearStore.Ward, getNearStore.Province, getNearStore.keyWord);
                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
