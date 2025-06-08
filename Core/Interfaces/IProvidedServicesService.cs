using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IProvidedServicesService
{
    public ProvidedService GetProvidedServiceById(int providedServiceId);
}