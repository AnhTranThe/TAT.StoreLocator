using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Response.Review
{
    public class GetReviewByUserAndStoreRequestModel
    {
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? StoreId { get; set; }
    }
}
