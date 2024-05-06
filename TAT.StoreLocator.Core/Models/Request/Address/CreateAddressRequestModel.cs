using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Address
{
    public class CreateAddressRequestModel
    {
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? RoadName { get; set; }
        [Required]
        public string? Province { get; set; }
        [Required]
        public string? District { get; set; }
        [Required]
        public string Ward { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public decimal latitude { get; set; } = 0;
        public decimal longitude { get; set; } = 0;


    }
}
