using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
       
        public ReviewController (IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("create")]
        public  async Task<IActionResult> CreateReview([FromBody]CreateReviewRequestModel request)
        {
            var response = await _reviewService.CreateReviewAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }



    }
}
