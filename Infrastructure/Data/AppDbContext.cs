using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataAccess.Configurations;
using Microsoft.AspNetCore.Identity;
namespace DataAccess.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }
    public DbSet<Master> Masters { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<ProvidedService> ProvidedServices { get; set; }
    public DbSet<WorkingMaster> WorkingMasters { get; set; }
    public DbSet<MasterService> MasterServices { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<Client> Clients { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new MasterConfiguration());
        builder.ApplyConfiguration(new AppointmentConfiguration());
        builder.ApplyConfiguration(new ProvidedServiceConfiguration());
        builder.ApplyConfiguration(new WorkingMasterConfiguration());
        builder.ApplyConfiguration(new MasterServiceConfiguration());
        builder.ApplyConfiguration(new SalaryConfiguration());
        builder.ApplyConfiguration(new ClientConfiguration());
    }
}