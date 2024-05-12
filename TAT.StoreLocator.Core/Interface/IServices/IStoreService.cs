using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Store;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IStoreService
    {
        //Create
        Task<CreateStoreResponseModel> CreateStoreAsync(CreateStoreRequestModel request);

        //GetAllStore
        Task<BaseResponseResult<List<StoreResponseModel>>> GetAllStoreAsync();

    }
}
