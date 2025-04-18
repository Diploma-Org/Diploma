using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Entities.Configurations;

public class WorkingMasterConfiguration : IEntityTypeConfiguration<WorkingMaster>
{
    public void Configure(EntityTypeBuilder<WorkingMaster> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}