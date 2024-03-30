using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Response.Review
{
    public class ReviewResponseModel
    {
        public string? Id { get; set; }
        public string? Content { get; set; }

        public int RatingValue { get; set; }
        public EReviewStatus Status { get; set; }

        public string? UserId { get; set; }

        public string? ProductId { get; set; }


        public string? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
