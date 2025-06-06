using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
namespace DataAccess.Configurations;

public class MasterServiceConfiguration : IEntityTypeConfiguration<MasterService>
{
    public void Configure(EntityTypeBuilder<MasterService> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}