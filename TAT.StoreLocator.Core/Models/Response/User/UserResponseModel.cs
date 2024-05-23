using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Response.User
{
    public class UserResponseModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public EGenderType Gender { get; set; }
        public string Dob { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int WishlistProductsCount { get; set; } = 0;
        public int WishlistStoresCount { get; set; } = 0;
        public int ReviewProductsCount { get; set; } = 0;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public ICollection<string>? Roles { get; set; }
    }
}