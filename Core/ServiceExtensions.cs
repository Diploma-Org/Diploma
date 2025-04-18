using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace BusinessLogic;

public static class ServiceExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    }
}
