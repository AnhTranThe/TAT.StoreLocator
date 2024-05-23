using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("gets")]
        public async Task<IActionResult> GetProfile()
        {
            Core.Common.BaseResponseResult<Core.Models.Response.User.UserResponseModel> check = await _profileService.GetByProfile();
            return Ok(check);
        }
    }
}