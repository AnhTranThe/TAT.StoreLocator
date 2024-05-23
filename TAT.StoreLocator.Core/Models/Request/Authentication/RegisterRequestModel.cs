using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Authentication
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "First name cannot be empty")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name cannot be empty")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email cannot be empty"), EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password cannot be empty")]
        public string? ConfirmPassword { get; set; }
    }
}