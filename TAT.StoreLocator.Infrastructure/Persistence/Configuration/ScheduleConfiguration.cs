using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Domain.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{

    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            _ = builder.HasOne(x => x.Store).WithMany(x => x.Schedules).HasForeignKey(x => x.StoreId).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
