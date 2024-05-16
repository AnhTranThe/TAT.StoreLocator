using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IProfileService
    {
        Task<BaseResponseResult<UserResponseModel>> GetByProfile();
    }
}
