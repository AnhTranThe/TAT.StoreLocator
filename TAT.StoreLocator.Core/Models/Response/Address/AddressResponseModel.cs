using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Response.Address
{
    public class AddressResponseModel
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? RoadName { get; set; }

        [Required]
        public string? Province { get; set; }

        [Required]
        public string? District { get; set; }

        [Required]
        public string? Ward { get; set; }

        public string? PostalCode { get; set; }
    }
}