using System.ComponentModel.DataAnnotations;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Request.Review
{
    public class UpdateReviewRequestModel
    {


        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string TypeId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int RatingValue { get; set; } = 0;

        public EReviewStatus Status { get; set; } = EReviewStatus.Approved;
        //public string? UpdatedBy { get; set; }
    }
}