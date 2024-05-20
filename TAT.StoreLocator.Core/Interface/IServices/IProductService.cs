using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IProductService
    {
        Task<BasePaginationResult<ProductResponseModel>> SearchProductAsync(SearchProductPagingRequestModel request);
        Task<BaseResponseResult<ProductResponseModel>> GetById(string Id);
        Task<BasePaginationResult<ProductResponseModel>> GetByIdStore(string StoreId, BasePaginationRequest request);
        Task<BasePaginationResult<ProductResponseModel>> GetListProductAsync(BasePaginationRequest request);
        Task<BaseResponse> AddProduct(ProductRequestModel request);
        Task<BaseResponse> UpdateProduct(string Id,ProductRequestModel request);


    }
}
