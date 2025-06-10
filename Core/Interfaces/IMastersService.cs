using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IMastersService
{
    List<Master> GetMasters();
    Master GetMasterById(int masterId);
    List<WorkingMaster> GetWorkingMasters();
    List<WorkingMaster> GetMastersByDate(DateTime date);
    void AddWorkingMaster(int masterId, DateTime date);
    void RemoveWorkingMaster(int masterId);
    void RemoveMaster(int masterId);
    void AddMaster(string name, string surname, string phone, int wagePercent);
    void UpdateMaster(int masterId, string name, string surname, string phone, int wagePercent);
}