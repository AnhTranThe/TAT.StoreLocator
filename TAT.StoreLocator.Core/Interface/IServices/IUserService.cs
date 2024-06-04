using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Request.Photo;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Role;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IUserService
    {
        Task<BasePaginationResult<UserResponseModel>> GetListUserAsync(BasePaginationRequest request);

        Task<BasePaginationResult<UserResponseModel>> SearchUserAsync(SearchUserPagingRequestModel request);

        Task<BaseResponseResult<UserResponseModel>> GetById(string id);

        Task<UpdateUserResponseModel> UpdateUserAsync(UpdateUserRequestModel request);

        Task<BaseResponse> UpdateUserPhoto(PhotoUploadProfileRequestModel request);

        Task<AssignRoleResponseModel> RoleAssign(AssignRoleRequestModel request);

        Task<BaseResponse> Delete(string id);

        Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequestModel request);

        Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequestModel request);

        Task<BaseResponse> ChangeStatusUser(ChangeStatusUserRequestModel request);
    }
}