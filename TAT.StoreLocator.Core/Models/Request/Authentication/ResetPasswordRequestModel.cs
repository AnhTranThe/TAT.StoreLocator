using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Authentication
{
    public class ResetPasswordRequestModel
    {
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Mật khẩu mới")]
        public string? NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        public string? ConfirmNewPassword { get; set; }


    }
}
