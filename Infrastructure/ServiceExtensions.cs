using Microsoft.Extensions.DependencyInjection;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
namespace DataAccess;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));
    }
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
