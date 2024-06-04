using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class UpdateStoreResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public StoreResponseModel StoreResponseModel { get; set; } = new StoreResponseModel();
    }
}