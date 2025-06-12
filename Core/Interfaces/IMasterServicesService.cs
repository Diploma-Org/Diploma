using BusinessLogic.DTOs;
using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IMasterServicesService
{
    public void AddMasterServiceFromList(int idMaster, int idProvidedService);
    void DeleteService(int idMaster, int idService);
    void DeleteAllMasterServices(int idMaster);
    public List<MasterService> GetMasterServices(int masterId);
    public MasterServicesAndMasterDto GetSevicesToDisplayForMaster(int? masterId);
}