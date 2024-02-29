using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;
using static TAT.StoreLocator.Domain.Helpers.Enums;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("Reviews")]
    public class Review : BaseEntity
    {
        [Required(ErrorMessage = "Review content must contain at least 2 characters")]
        public string? Content { get; set; }
        public int RatingValue { get; set; }
        [Required]
        public EReviewStatus Status { get; set; } = EReviewStatus.Pending;

        // relationship

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }




    }
}
