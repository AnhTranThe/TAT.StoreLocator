using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/admin/store")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.RoleAdminName)]
    public class AdminStoreController : ControllerBase
    {

        //Depency Injection
        private readonly IStoreService _storeService;

        //Constructor
        public AdminStoreController(IStoreService storeService)
        {
            _storeService = storeService;

        }


        [HttpPost("create")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> CreateStore([FromForm] CreateStoreRequestModel request)
        {
            try
            {

                Core.Models.Response.Store.CreateStoreResponseModel response = await _storeService.CreateStoreAsync(request);


                return response == null
                    ? StatusCode(500, "Failed to create store")
                    : Ok(response);
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






    }
}
