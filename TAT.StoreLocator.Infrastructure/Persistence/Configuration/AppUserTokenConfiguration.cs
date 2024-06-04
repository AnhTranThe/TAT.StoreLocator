using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AppUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
        {
            _ = builder.ToTable("UserTokens");
            _ = builder.HasKey(x => x.UserId);
        }
    }
}