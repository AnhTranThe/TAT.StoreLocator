using Microsoft.AspNetCore.Identity;

namespace TAT.StoreLocator.Core.Entities
{
    public class Role : IdentityRole<string>
    {
        public string? NormalizeName { get; set; }
    }
}
