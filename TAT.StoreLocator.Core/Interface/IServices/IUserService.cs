using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Role;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IUserService
    {
        Task<BasePaginationResult<UserResponseModel>> GetUsersPaging(GetUserPagingRequestModel request);
        Task<BaseResponseResult<UserResponseModel>> GetById(string id);
        Task<EditUserResponseModel> UpdateUserAsync(EditUserRequestModel request);
        Task<AssignRoleResponseModel> RoleAssign(AssignRoleRequestModel request);
        Task<BaseResponse> Delete(string id);
        Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequestModel request);
        Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequestModel request);
        Task<BaseResponse> UpdateJwtUserInfo(UpdateJwtUserInfoRequestModel request);






    }
}
