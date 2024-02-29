using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AppUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        {
            _ = builder.ToTable("UserTokens");
            _ = builder.HasKey(x => x.UserId);

        }
    }
}
