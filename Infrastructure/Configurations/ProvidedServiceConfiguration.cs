using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
namespace DataAccess.Configurations;

public class ProvidedServiceConfiguration : IEntityTypeConfiguration<ProvidedService>
{
    public void Configure(EntityTypeBuilder<ProvidedService> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}