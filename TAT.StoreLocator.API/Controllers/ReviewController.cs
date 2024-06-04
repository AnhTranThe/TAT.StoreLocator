using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Review;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewRequestModel request)
        {

            try
            {
                BaseResponse response = await _reviewService.CreateReviewAsync(request);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("update/{reviewId}")]
        public async Task<IActionResult> UpdateReview(string reviewId, [FromBody] ReviewRequestModel request)
        {
            try
            {
                BaseResponseResult<Core.Models.Response.Review.ReviewResponseModel> response = await _reviewService.UpdateReviewAsync(reviewId, request);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getReviewByUser/{userId}")]

        public async Task<IActionResult> GetReviewByUserId(string userId, [FromQuery] BaseReviewFilterRequest filterRequest, [FromQuery] BasePaginationRequest paginationRequest)
        {
            BasePaginationResult<Core.Models.Response.Review.ReviewResponseModel> response = await _reviewService.GetReviewsByUserIdAsync(filterRequest, paginationRequest);

            return response.Data != null
                ? Ok(new
                {
                    data = response.Data,
                    pageSize = response.PageSize,
                    pageIndex = response.PageIndex,
                    totalCount = response.TotalCount,
                    searchString = response.SearchString
                })
                : (IActionResult)StatusCode(500, "Can not find review by userId");
        }

        [HttpGet("getReviewByStore")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewByStoreId([FromQuery] BaseReviewFilterRequest filterRequest, [FromQuery] BasePaginationRequest paginationRequest)
        {
            BasePaginationResult<Core.Models.Response.Review.ReviewResponseModel> response = await _reviewService.GetReviewsByStoreIdAsync(filterRequest, paginationRequest);
            return response.Data != null
                ? Ok(new
                {
                    data = response.Data,
                    pageSize = response.PageSize,
                    pageIndex = response.PageIndex,
                    totalCount = response.TotalCount,
                    searchString = response.SearchString
                })
                : (IActionResult)StatusCode(500, "Can not find review by storeId");
        }
    }
}