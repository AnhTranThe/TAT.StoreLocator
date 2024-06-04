using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            _ = builder.HasOne(x => x.Address).WithOne(x => x.Store).HasForeignKey<Store>(x => x.AddressId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}