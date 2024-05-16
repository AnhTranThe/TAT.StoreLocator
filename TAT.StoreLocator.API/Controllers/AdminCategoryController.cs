using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Helpers;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.RoleAdminName)]
    public class AdminCategoryController : ControllerBase
    {

        public AdminCategoryController()
        {

        }

    }
}
