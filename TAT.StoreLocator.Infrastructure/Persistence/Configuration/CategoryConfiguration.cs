using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            _ = builder.HasOne(x => x.ParentCategory).WithMany(x => x.ChildrenCategories).HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.Cascade);
            _ = builder.HasOne(x => x.Gallery).WithOne(x => x.Category).HasForeignKey<Gallery>(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
