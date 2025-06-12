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

    public IEnumerable<ProvidedService> GetProvidedServices()
    {
        return _providedServiceRepository.GetAll()
               ?? throw new InvalidOperationException("No provided services found.");
    }
    public void AddProvidedService(ProvidedService providedService)
    {
        if (providedService == null)
        {
            throw new ArgumentNullException(nameof(providedService), "Provided service cannot be null.");
        }

        _providedServiceRepository.Insert(providedService);
        _providedServiceRepository.Save();
    }
    public void UpdateProvidedService(ProvidedService providedService)
    {
        if (providedService == null)
        {
            throw new ArgumentNullException(nameof(providedService), "Provided service cannot be null.");
        }

        _providedServiceRepository.Update(providedService);
        _providedServiceRepository.Save();
    }
    public void DeleteProvidedService(int providedServiceId)
    {
        var providedService = GetProvidedServiceById(providedServiceId);
        _providedServiceRepository.Delete(providedService);
        _providedServiceRepository.Save();
    }
}