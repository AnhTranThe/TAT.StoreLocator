using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {

            _ = builder.HasOne(x => x.Location).WithOne(x => x.Address).HasForeignKey<Location>(x => x.AddressId).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(x => x.User).WithOne(x => x.Address).HasForeignKey<User>(x => x.AddressId).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(x => x.Store).WithOne(x => x.Address).HasForeignKey<Store>(x => x.AddressId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
