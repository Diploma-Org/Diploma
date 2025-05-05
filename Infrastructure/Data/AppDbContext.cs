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
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<ProvidedService> ProvidedServices { get; set; }
    public DbSet<WorkingMaster> WorkingMasters { get; set; }
    public DbSet<MasterService> MasterServices { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new MasterConfiguration());
        builder.ApplyConfiguration(new AppointmentConfiguration());
        builder.ApplyConfiguration(new ProvidedServiceConfiguration());
        builder.ApplyConfiguration(new AdminConfiguration());
        builder.ApplyConfiguration(new WorkingMasterConfiguration());
        builder.ApplyConfiguration(new MasterServiceConfiguration());
    }
}