using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IMastersService
{
    List<Master> GetMasters();
    List<WorkingMaster> GetWorkingMasters();
    List<WorkingMaster> GetMastersByDate(DateTime date);
    void AddWorkingMaster(int masterId, DateTime date);
    void RemoveWorkingMaster(int masterId);
    void RemoveMaster(int masterId);
    void AddMaster(string name, string surname, string phone);
}