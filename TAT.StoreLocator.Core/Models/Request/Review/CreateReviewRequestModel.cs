using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Review
{
    public class CreateReviewRequestModel
    {
        [Required]
        public string StoreId { get; set; } 
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? ProductId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public int RatingValue { get; set; }

     

    }
}
