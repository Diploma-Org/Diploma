using BusinessLogic.DTOs;
using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IMasterServicesService
{
    public void AddMasterServiceFromList(int idMaster, int idProvidedService);
    void AddService(string name, decimal price);
    void DeleteService(int idMaster, int idService);
    public List<MasterService> GetMasterServices(int masterId);
    public MasterServicesAndMasterDto GetSevicesToDisplayForMaster(int? masterId);
}