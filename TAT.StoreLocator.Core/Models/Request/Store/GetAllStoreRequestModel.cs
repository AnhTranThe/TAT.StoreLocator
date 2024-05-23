using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Store
{
    public class GetAllStoreRequestModel
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}