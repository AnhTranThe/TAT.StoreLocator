using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Address
{
    public class CreateAddressRequestModel
    {
        [Required]
        public string? RoadName { get; set; }
        [Required]
        public string? Province { get; set; }
        [Required]
        public string? District { get; set; }
        [Required]
        public string Ward { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;


    }
}
