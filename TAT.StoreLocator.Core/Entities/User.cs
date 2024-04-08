using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Entities
{
    public class User : IdentityUser<string>
    {

        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        private string _FullName = string.Empty;
        [Required]
        public string FullName
        {
            get => _FullName;
            set
            {
                _FullName = value;
                _FullName = LastName + " " + FirstName;
            }
        }
        public EGenderType Gender { get; set; } = EGenderType.NotInformation;
        public DateTimeOffset Dob { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        // relationship 
        public Address? Address { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }




    }
}
