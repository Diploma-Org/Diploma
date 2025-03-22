using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Entities.Configurations;

public class AppoinmentConfiguration : IEntityTypeConfiguration<Appoinment>
{
    public void Configure(EntityTypeBuilder<Appoinment> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}