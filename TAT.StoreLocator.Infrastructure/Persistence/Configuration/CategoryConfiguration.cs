using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            _ = builder.HasOne(x => x.Gallery).WithOne(x => x.Category).HasForeignKey<Category>(x => x.GalleryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}