using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class StoreResponseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RoadName { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? PostalCode { get; set; }
        public decimal latitude { get; set; }
        public decimal longtitude { get; set; }
        public List<MapGalleryStoreResponse> MapGalleryStores { get; set; } = new List<MapGalleryStoreResponse>();
    }

    public class MapGalleryStoreResponse
    {
        public string? GalleryId { get; set; }
        public string? FileName { get; set; }
        public string? Url { get; set; }
        public bool IsThumbnail { get; set; }
    }
}
