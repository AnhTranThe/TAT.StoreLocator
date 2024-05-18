using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Request.Review
{
    public class UpdateReviewRequestModel
    {
        [Required]
        public string? StoreId { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? ProductId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public int? RatingValue { get; set; }
        public EReviewStatus? Status { get; set; }
        //public string? UpdatedBy { get; set; }
    }

}
