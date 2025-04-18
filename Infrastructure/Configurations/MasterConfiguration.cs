using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Entities.Configurations;

public class MasterConfiguration : IEntityTypeConfiguration<Master>
{
    public void Configure(EntityTypeBuilder<Master> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}