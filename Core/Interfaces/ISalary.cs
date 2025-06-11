using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface ISalaryService
{
    void AddSalary(int masterId);
    void RemoveSalary(int masterId);
    void IncreaseWage(Master master, Salary salary, ProvidedService providedService, DateTime date);
    void DecreaseWage(Master master, Salary salary, ProvidedService providedService, DateTime date);
    void DecreaseWage(int masterId, float Withdrawal);
    List<Salary> GetSalaries(List<Master> masters);
}