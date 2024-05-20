using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Category;
using TAT.StoreLocator.Core.Models.Response.Category;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface ICategoryService
    {
        //Task<int> Create(CategoryCreateRequest request);
        //Task<int> Update(CategoryUpdateRequest request);
        //Task<int> Delete(int providerId);
        //Task<CategoryViewModel> GetStoreById(int providerId);
        //Task<List<CategoryViewModel>> GetAll();
        //Task<List<CategoryViewModel>> Search(string search);
        Task<BaseResponseResult<CategoryResponseModel>> GetById(string Id);
        Task<BasePaginationResult<CategoryResponseModel>> GetListAsync(BasePaginationRequest request);
        Task<BasePaginationResult<CategoryResponseModel>> GetListParentCategoryAsync(BasePaginationRequest request);
        Task<BasePaginationResult<CategoryResponseModel>> GetListSubCategoryAsync(BasePaginationRequest request);
        Task<BaseResponse> Add(CategoryRequestModel request);
        Task<BaseResponse> Update(string id, CategoryRequestModel request);
    }
}
