using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
namespace BusinessLogic.Services;

public class ProvidedServicesService : IProvidedServicesService
{
    private readonly IRepository<ProvidedService> _providedServiceRepository;

    public ProvidedServicesService(IRepository<ProvidedService> providedServiceRepository)
    {
        _providedServiceRepository = providedServiceRepository;
    }

    public ProvidedService GetProvidedServiceById(int providedServiceId)
    {
        return _providedServiceRepository.GetById(providedServiceId)
               ?? throw new KeyNotFoundException($"Provided service with ID {providedServiceId} not found.");
    }    
}