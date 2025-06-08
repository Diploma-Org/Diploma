using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
namespace DataAccess.Configurations
{
    public class DailyWageConfiguration : IEntityTypeConfiguration<DailyWage>
    {
        public void Configure(EntityTypeBuilder<DailyWage> builder)
        {
            builder
                .HasKey(x => x.Id);
        }
    }
}