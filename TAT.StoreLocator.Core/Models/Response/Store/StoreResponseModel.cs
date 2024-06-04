using TAT.StoreLocator.Core.Models.Response.Review;

namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class StoreResponseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressResponseModel? Address { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }

        public RatingStore RatingStore { get; set; } = new RatingStore();

        public List<MapGalleryStoreResponseModel> MapGalleryStores { get; set; } = new List<MapGalleryStoreResponseModel>();

        public List<ReviewResponseModel> Reviews { get; set; } = new List<ReviewResponseModel>();
    }

    public class MapGalleryStoreResponseModel
    {
        public string? GalleryId { get; set; }
        public string? Key { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string? Url { get; set; }
        public bool IsThumbnail { get; set; }
        public string? StoreId { get; set; }
    }

    public class AddressResponseModel
    {
        public string? Id { get; set; }
        public string? RoadName { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? PostalCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class RatingStore
    {
        public int? NumberRating { get; set; }
        public double? PointOfRating { get; set; }
    }
}