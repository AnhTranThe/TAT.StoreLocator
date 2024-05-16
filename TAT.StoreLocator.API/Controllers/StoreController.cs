using Microsoft.AspNetCore.Mvc;
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
                var response = await _storeService.CreateStoreAsync(request);
                if (response == null)
                {
                    return StatusCode(500, "Failed to create store");
                }

                return CreatedAtAction(nameof(GetAllStore), new { storeid = response.Id }, response.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllStore()
        {
            try
            {
                var response = await _storeService.GetAllStoreAsync();
                if (response != null && response.Success)
                {
                    return Ok(response.Data);
                }
                return StatusCode(500, "Failed to get stores");
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
                var request = new GetDetailStoreRequestModel { Id = storeId };
                var response = await _storeService.GetDetailStoreAsync(storeId);
                if (response.Success)
                {
                    return Ok(response.Data);
                }
                else
                {
                    return NotFound(response.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //[HttpGet("getDetail/{storeId}")]
        //public async Task<IActionResult> GetDetailStore(string storeId)
        //{
        //    try
        //    {
        //        //var request = new GetDetailStoreRequestModel { Id = storeId };
        //        var response = await _storeService.GetDetailStoreAsync(storeId); // test
        //        if (response.Success)
        //        {
        //            return Ok(response.Data);
        //        }
        //        return NotFound(response.Message);
        //        //        //else
        //        //        //{
        //        //        //    return NotFound(response.Message);
        //        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
