using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Response.Role;

namespace TAT.StoreLocator.Core.Models.Response.User
{
    public class UserWithRolesResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public UserResponseModel UserResponse { get; set; } = new UserResponseModel();
        public List<RoleResponseModel> RoleResponse { get; set; } = new List<RoleResponseModel>();
    }
}
