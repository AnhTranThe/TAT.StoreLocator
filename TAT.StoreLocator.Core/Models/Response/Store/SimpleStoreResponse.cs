using TAT.StoreLocator.Core.Models.Response.Review;

namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class SimpleStoreResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressResponseModel? Address { get; set; }
        public List<SimpleProductResponse> Products { get; set; } = new List<SimpleProductResponse>();
        public List<ReviewResponseModel> Reviews { get; set; } = new List<ReviewResponseModel>();
        public List<MapGalleryStoreResponseModel> Images { get; set; } = new List<MapGalleryStoreResponseModel>();
    }

    public class SimpleProductResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? StoreId { get; set; }
    }
}