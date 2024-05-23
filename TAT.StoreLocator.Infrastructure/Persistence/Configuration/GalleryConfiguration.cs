using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class GalleryConfiguration : IEntityTypeConfiguration<Gallery>
    {
        public void Configure(EntityTypeBuilder<Gallery> builder)
        {
            _ = builder.HasOne(x => x.User).WithOne(x => x.Gallery).HasForeignKey<Gallery>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}