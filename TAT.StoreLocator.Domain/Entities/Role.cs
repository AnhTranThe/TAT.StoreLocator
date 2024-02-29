using Microsoft.AspNetCore.Identity;

namespace TAT.StoreLocator.Domain.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string? NormalizeName { get; set; }
    }
}
