using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Authentication
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email or Username cannot be empty")]
        public string? EmailOrUserName { get; set; }


        [Required(ErrorMessage = "Password cannot be empty")]
        public string? Password { get; set; }
    }
}
