using TAT.StoreLocator.Core.Models.Response.Role;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Models.Response.UserRole
{
    public class UserRoleResponseModel
    {
        public UserResponseModel? UserResponseModel { get; set; }
        public List<RoleResponseModel>? RoleResponseModels { get; set; }
    }
}