using System.ComponentModel.DataAnnotations;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Request.Review
{
    public class ReviewRequestModel
    {
        [Required]
        public string? TypeId { get; set; }
        [Required]
        public string? Content { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public int RatingValue { get; set; }

        public EReviewStatus Status { get; set; } = EReviewStatus.Approved;
    }
}