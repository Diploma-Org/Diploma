using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IProvidedServicesService
{
    ProvidedService GetProvidedServiceById(int providedServiceId);
    IEnumerable<ProvidedService> GetProvidedServices();
    void AddProvidedService(ProvidedService providedService);
    void UpdateProvidedService(ProvidedService providedService);
    void DeleteProvidedService(int providedServiceId);
}