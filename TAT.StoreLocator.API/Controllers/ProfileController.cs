using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("Profile")]
        //[Authorize(Roles = GlobalConstants.RoleUserName)]
        public async Task<IActionResult> GetProfile()
        {
            var check = await _profileService.GetByProfile();
            return Ok(check);
        }
    }
}
