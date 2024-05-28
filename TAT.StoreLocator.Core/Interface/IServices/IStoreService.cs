using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Store;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IStoreService
    {
        //Create
        Task<CreateStoreResponseModel> CreateStoreAsync(StoreRequestModel request);

        //GetAllStore
        Task<BasePaginationResult<StoreResponseModel>> GetAllStoreAsync(BasePaginationRequest paginationRequest);

        ////GetDetailStore
        Task<BaseResponseResult<StoreResponseModel>> GetDetailStoreAsync(string storeId);

        //UpdateStore
        Task<BaseResponseResult<StoreResponseModel>> UpdateStoreAsync(string storeId, StoreRequestModel request);

        ////DeleteStore
        Task<BaseResponse> DeleteStoreAsync(string storeId);

        /// <summary>
        /// Get the near Stroe by input distric
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        Task<BaseResponseResult<List<SimpleStoreResponse>>> GetTheNearestStore(GetNearStoreRequestModel getNearStoreRequest, BasePaginationRequest paginationRequest);
    }
}