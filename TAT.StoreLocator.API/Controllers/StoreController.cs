using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Store;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreController : ControllerBase
    {
        //Depency Injection
        private readonly IStoreService _storeService;

        //Constructor
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;

        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllStore([FromQuery] BasePaginationRequest paginationRequest)
        {
            try
            {

                BasePaginationResult<StoreResponseModel> response = await _storeService.GetAllStoreAsync(paginationRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getDetail/{storeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailStore(string storeId)
        {
            try
            {

                Core.Common.BaseResponseResult<Core.Models.Response.Store.StoreResponseModel> response = await _storeService.GetDetailStoreAsync(storeId);
                return response.Success ? Ok(response.Data) : NotFound(response.Message);
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
                BaseResponseResult<List<Core.Models.Response.Store.SimpleStoreResponse>> response = await _storeService.GetTheNearestStore(getNearStore.District, getNearStore.Ward, getNearStore.Province, getNearStore.keyWord);
                return response.Success ? Ok(response) : (IActionResult)Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
