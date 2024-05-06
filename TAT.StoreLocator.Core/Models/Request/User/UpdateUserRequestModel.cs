using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TAT.StoreLocator.Core.Common;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class UpdateUserRequestModel : BaseRequest
    {

        [Required]
        [MinLength(2)]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        public IFormFile? NewFile { get; set; }

        public EGenderType Gender { get; set; }

        public DateTimeOffset? Dob { get; set; }
        public string? ImgUrl { get; set; }
        public bool IsActiveAddress { get; set; } = false;
        public string? RoadName { get; set; } = string.Empty;
        public string? Province { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? Ward { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public decimal latitude { get; set; } = 0;
        public decimal longitude { get; set; } = 0;

    }
}
