using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Domain.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            _ = builder.HasOne(x => x.ParentCategory).WithMany(x => x.ChildrenCategories).HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(x => x.Gallery).WithMany(x => x.Categories).HasForeignKey(x => x.GalleryId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
