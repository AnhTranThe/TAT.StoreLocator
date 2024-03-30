using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class LoginUserRequestModel
    {
        [Required(ErrorMessage = "Email or Username cannot be empty")]
        public string? EmailOrUserName { get; set; }


        [Required(ErrorMessage = "Password cannot be empty")]
        public string? Password { get; set; }
    }
}
