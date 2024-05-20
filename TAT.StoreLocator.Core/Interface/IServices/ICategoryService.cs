using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Category;
using TAT.StoreLocator.Core.Models.Response.Category;

namespace TAT.StoreLocator.Core.Interface.IServices
{
        public interface ICategoryService
        {
                /// <summary>
                /// getById Category
                /// </summary>
                /// <param name="Id"></param>
                /// <returns> Category</returns>
                Task<BaseResponseResult<CategoryResponseModel>> GetById(string Id);
                /// <summary>
                /// GetListCategory
                /// </summary>
                /// <param name="request"></param>
                /// <returns> List category with pagination </returns>
                Task<BasePaginationResult<CategoryResponseModel>> GetListAsync(BasePaginationRequest request);
                Task<BasePaginationResult<CategoryResponseModel>> GetListParentCategoryAsync(BasePaginationRequest request);
                Task<BasePaginationResult<CategoryResponseModel>> GetListSubCategoryAsync(BasePaginationRequest request);
                /// <summary>
                /// add new Category
                /// </summary>
                /// <param name="request"></param>
                /// <returns> true or false </returns>
                Task<BaseResponse> Add(CategoryRequestModel request);
                /// <summary>
                /// Update Category
                /// </summary>
                /// <param name="Id"></param>
                /// <param name="request"></param>
                /// <returns> true or false  </returns>
                Task<BaseResponse> Update(string Id, CategoryRequestModel request);
        }
}
