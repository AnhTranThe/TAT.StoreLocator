using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            _ = builder.ToTable("Users");
        }
    }
}