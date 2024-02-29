using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Domain.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            _ = builder.ToTable("Roles");
            _ = builder.HasKey(x => x.Id);



        }
    }
}
