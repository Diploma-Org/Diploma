using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Entities.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}