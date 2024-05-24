using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Photo;
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Response.Gallery;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GalleryController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        public GalleryController(IPhotoService photoService)
        {
            _photoService = photoService;

        }
        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] BasePaginationRequest request)
        {
            request ??= new BasePaginationRequest();

            try
            {
                BasePaginationResult<GalleryResponseModel> response = await _photoService.GetListImagesAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("getlistbyid")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListById([FromQuery] GetListPhotoByIdRequestModel request)
        {
            request ??= new GetListPhotoByIdRequestModel();
            try
            {
                BasePaginationResult<GalleryResponseModel> response = await _photoService.GetListImagesById(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("add")]
        public async Task<ActionResult> Add([FromBody] UploadPhotoRequestModel request)
        {
            try
            {

                if (request.FileUpload == null)
                {
                    return BadRequest("File upload not found");
                }
                BaseResponse Response = await _photoService.CreateImage(request);

                return !Response.Success ? BadRequest(Response.Message) : Ok(Response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("update/{Id}")]
        public async Task<IActionResult> Update(string Id, [FromBody] UpdatePhotoRequestModel request)
        {

            try
            {
                if (request == null || string.IsNullOrEmpty(Id))
                {
                    return BadRequest(new BaseResponse
                    {
                        Success = false,
                        Message = "Invalid request data."
                    });
                }

                BaseResponse response = await _photoService.UpdateImage(Id, request);
                return !response.Success ? BadRequest(response) : Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {

            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return BadRequest(new BaseResponse
                    {
                        Success = false,
                        Message = "Invalid request data."
                    });
                }

                BaseResponse response = await _photoService.RemoveImage(Id);
                return !response.Success ? BadRequest(response) : Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
