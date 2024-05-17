﻿using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
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
        Task<BaseResponseResult<StoreResponseModel>> GetDetailStoreAsync(string storeId);

        //UpdateStore
        Task<BaseResponseResult<StoreResponseModel>>UpdateStoreAsync ( string storeId, UpdateStoreRequestModel request );

        ////DeleteStore
        Task<BaseResponse> DeleteStoreAsync(string storeId);
    }
}
