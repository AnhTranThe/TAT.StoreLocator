using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AppUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
        {
            _ = builder.ToTable("UserClaims");
            _ = builder.HasKey(x => x.Id);
        }
    }
}