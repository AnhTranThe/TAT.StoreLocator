using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AppUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
        {
            _ = builder.ToTable("UserLogins");
            _ = builder.HasKey(x => new { x.ProviderKey, x.LoginProvider });

        }
    }
}
