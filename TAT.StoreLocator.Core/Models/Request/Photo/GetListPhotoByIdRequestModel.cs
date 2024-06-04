using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Request.Photo
{
    public class GetListPhotoByIdRequestModel : BasePaginationRequest
    {
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

    }
}
