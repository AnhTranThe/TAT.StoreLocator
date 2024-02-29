using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            _ = builder.ToTable("UserRoles");
            _ = builder.HasKey(x => new { x.RoleId, x.UserId });

        }
    }
}
