using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface ISalaryService
{
    void AddSalary(int masterId, decimal earnings);
    void RemoveSalary(int masterId);
    void IncreaseWage(int masterId, decimal newEarnings);
    void DecreaseWage(int masterId, decimal Withdrawal);
    List<Salary> GetSalaries(List<Master> masters);
}