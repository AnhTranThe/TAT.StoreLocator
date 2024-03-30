using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {

            _ = builder.HasOne(x => x.Address).WithOne(x => x.Location).HasForeignKey<Location>(x => x.AddressId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
