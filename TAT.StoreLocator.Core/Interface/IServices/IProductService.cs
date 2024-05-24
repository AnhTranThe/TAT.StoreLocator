using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Response.Product;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IProductService
    {
        Task<BasePaginationResult<ProductResponseModel>> SearchProductAsync(SearchProductPagingRequestModel request);

        /// <summary>
        /// GetProductById
        /// </summary>
        /// <param name="Id"></param>
        /// <returns> Product detail </returns>
        Task<BaseResponseResult<ProductResponseModel>> GetById(string Id);

        /// <summary>
        /// GetProductbyStoreId
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="request"></param>
        /// <returns>Product detail of storeID</returns>
        Task<BasePaginationResult<ProductResponseModel>> GetByIdStore(string StoreId, BasePaginationRequest request);

        /// <summary>
        /// GetListProduct
        /// </summary>
        /// <param name="request"></param>
        /// <returns> all product with some detail</returns>
        Task<BasePaginationResult<ProductResponseModel>> GetListProductAsync(BasePaginationRequest request);

        /// <summary>
        /// add new product
        /// </summary>
        /// <param name="request"></param>
        /// <returns> boolean </returns>
        Task<BaseResponse> AddProduct(ProductRequestModel request);

        /// <summary>
        /// update propduct
        /// </summary>
        /// <param name="Id"></param>
        /// <param name=" request "></param>
        /// <returns> boolean </returns>
        Task<BaseResponse> UpdateProduct(string Id, UpdateProductRequestModel request);
    }
}