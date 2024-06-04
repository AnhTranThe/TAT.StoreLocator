using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Response.UserRole;

namespace TAT.StoreLocator.Core.Models.Response.Role
{
    public class RemoveFromRoleResponseModel
    {
        public BaseResponse? BaseResponse { get; set; }
        public UserRoleResponseModel? UserRoleResponse { get; set; }
    }
}