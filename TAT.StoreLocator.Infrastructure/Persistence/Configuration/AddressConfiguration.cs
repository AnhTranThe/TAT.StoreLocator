using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {

            _ = builder.HasOne(x => x.User).WithOne(x => x.Address).HasForeignKey<Address>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            _ = builder.HasOne(x => x.Store).WithOne(x => x.Address).HasForeignKey<Address>(x => x.StoreId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
