using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Review;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequestModel request)
        {
            BaseResponseResult<Core.Models.Response.Review.ReviewResponseModel> response = await _reviewService.CreateReviewAsync(request);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateReview(string reviewId, [FromBody] UpdateReviewRequestModel request)
        {
            try
            {
                BaseResponseResult<Core.Models.Response.Review.ReviewResponseModel> response = await _reviewService.UpdateReviewAsync(reviewId, request);
                return response.Success ? Ok(response.Data) : (IActionResult)StatusCode(500, response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getReviewByUserId/{userId}")]
        public async Task<IActionResult> GetReviewByUserId(string userId, [FromQuery] BaseReviewFilterRequest filterRequest, [FromQuery] BasePaginationRequest paginationRequest)
        {
            BasePaginationResult<Core.Models.Response.Review.ReviewResponseModel> response = await _reviewService.GetReviewByUserIdAsync(userId, filterRequest, paginationRequest);

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

        [HttpGet("getReviewByStoreId/{storeId}")]
        public async Task<IActionResult> GetReviewByStoreId(string storeId, [FromQuery] BaseReviewFilterRequest filterRequest, [FromQuery] BasePaginationRequest paginationRequest)
        {
            BasePaginationResult<Core.Models.Response.Review.ReviewResponseModel> response = await _reviewService.GetReviewByStoreIdAsync(storeId, filterRequest, paginationRequest);
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