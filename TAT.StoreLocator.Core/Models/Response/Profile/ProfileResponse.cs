using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Response.Profile
{
    public class ProfileResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public EGenderType Gender { get; set; }
        public string Dob { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}