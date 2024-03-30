using System.ComponentModel.DataAnnotations;
using TAT.StoreLocator.Core.Common;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class EditUserRequestModel : BaseRequest
    {

        [Required]
        [MinLength(2)]
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        public EGenderType Gender { get; set; }

        public DateTimeOffset Dob { get; set; }

        public string? AddressId { get; set; }



    }
}
