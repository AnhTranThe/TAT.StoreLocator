using System.ComponentModel.DataAnnotations;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class ChangePasswordRequestModel : BaseRequest
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? OldPassword { get; set; }

        [Required]
        public string? NewPassword { get; set; }

        [Required]
        public string? ConfirmNewPassword { get; set; }
    }
}