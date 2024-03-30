using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Response.UserRole;

namespace TAT.StoreLocator.Core.Models.Response.Role
{
    public class AssignRoleResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public UserRoleResponseModel UserRoleResponse { get; set; } = new UserRoleResponseModel();

    }
}
