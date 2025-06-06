using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
namespace DataAccess.Configurations;

public class WorkingMasterConfiguration : IEntityTypeConfiguration<WorkingMaster>
{
    public void Configure(EntityTypeBuilder<WorkingMaster> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}