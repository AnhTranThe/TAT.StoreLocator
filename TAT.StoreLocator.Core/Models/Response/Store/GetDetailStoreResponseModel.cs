namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class GetDetailStoreResponseModel
    {
        //public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public StoreResponseModel StoreResponseModel { get; set; } = new StoreResponseModel();

        public MapGalleryStoreResponseModel MapGalleryStore { get; set; } = new MapGalleryStoreResponseModel();
    }
}