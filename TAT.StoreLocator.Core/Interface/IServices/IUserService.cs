using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Role;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Role;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Core.Models.Response.UserRole;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IUserService
    {
        Task<GetUserResponseModel> GetUserAsync(BaseRequest model);

        Task<UserRoleResponseModel> GetUserWithRolesAsync(BaseRequest model);

        Task<PaginationUserResponseModel> GetAllAsync(PaginationUserRequestModel model);

        public UserResponseModel GetUserInfoFromJwt();

        public Task<AssignRoleResponseModel> AssignRoleAsync(AssignRoleRequestModel model);

        public Task<RemoveFromRoleResponseModel> RemoveFromRoleAsync(RemoveFromRoleRequestModel model);

        public Task<EditUserResponseModel> EditUserAsync(EditUserRequestModel model);

        public Task<DeleteUserResponseModel> DeleteUserAsync(BaseRequest model);

        public Task<UndeleteUserResponseModel> UndeleteUserAsync(BaseRequest model);

        public bool IsUserAdmin();
    }
}
