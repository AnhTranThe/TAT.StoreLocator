using Microsoft.AspNetCore.Http;

namespace TAT.StoreLocator.Core.Models.Request.Store
{
    public class CreateStoreRequestModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressStoreRequestModel? Address { get; set; }
        public ICollection<IFormFile>? files { get; set; }
    }
    public class AddressStoreRequestModel
    {
        public string? RoadName { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
