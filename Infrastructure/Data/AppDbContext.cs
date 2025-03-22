using DataAccess.Entities;
using DataAccess.Entities.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }
    public DbSet<Master> Masters { get; set; }
    public DbSet<Appoinment> Appoinments { get; set; }
    public DbSet<ProvidedService> ProvidedServices { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new MasterConfiguration());
        modelBuilder.ApplyConfiguration(new AppoinmentConfiguration());
        modelBuilder.ApplyConfiguration(new ProvidedServiceConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
    }
}