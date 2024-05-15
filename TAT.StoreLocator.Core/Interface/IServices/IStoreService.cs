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

        ////GetDetailStore
        //Task<BaseResponseResult<GetDetailStoreResponseModel>> GetDetailStoreAsync(GetDetailStoreRequestModel request);


        Task<BaseResponseResult<StoreResponseModel>> GetDetailStoreAsync(string storeId);
    }
}
