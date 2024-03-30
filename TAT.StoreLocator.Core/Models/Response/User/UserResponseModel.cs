using TAT.StoreLocator.Core.Models.Response.Address;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Response.User
{
    public class UserResponseModel
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public EGenderType Gender { get; set; }
        public DateTimeOffset? Dob { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public DateTimeOffset? CreateAt { get; set; }
        public string? CreateBy { get; set; }
        public DateTimeOffset? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public int WishlistProductsCount { get; set; } = 0;
        public int WishlistStoresCount { get; set; } = 0;
        public int ReviewProductsCount { get; set; } = 0;
        public string? RefreshToken { get; set; }
        public DateTimeOffset TokenCreated { get; set; }
        public DateTimeOffset TokenExpires { get; set; }
        public string? AddressId { get; set; }
        public ICollection<AddressResponseModel>? Address { get; set; }
        public ICollection<string>? Roles { get; set; }


    }
}
