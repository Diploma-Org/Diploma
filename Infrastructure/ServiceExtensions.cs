using Microsoft.Extensions.DependencyInjection;
using DataAccess.Interfaces;
using DataAccess.Repositories;
namespace DataAccess;

public static class ServiceExtensions
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
