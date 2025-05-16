using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
namespace BusinessLogic.Services;

public class SalaryService : ISalaryService
{
    private readonly IRepository<Master> _mastersRepository;
    private readonly IRepository<Salary> _salariesRepository;

    public SalaryService(
        IRepository<Master> mastersRepository,
        IRepository<Salary> salariesRepository)
    {
        _mastersRepository = mastersRepository;
        _salariesRepository = salariesRepository;
    }

    public void AddSalary(int masterId, decimal earnings)
    {
        var master = _mastersRepository.GetById(masterId);
        if (master == null)
            throw new ArgumentNullException($"Master with ID {masterId} not found.");

        var salary = new Salary
        {
            IdMaster = masterId,
            Earnings = earnings
        };

        _salariesRepository.Insert(salary);
        _salariesRepository.Save();
    }

    public void RemoveSalary(int masterId)
    {
        var salary = _salariesRepository.GetAll()
            .FirstOrDefault(s => s.IdMaster == masterId);

        if (salary == null)
            throw new ArgumentNullException($"Salary with ID {masterId} not found.");
        if (salary.Earnings > 0)
            throw new InvalidOperationException("Cannot remove a salary that is greater than 0.");

        _salariesRepository.Delete(salary);
        _salariesRepository.Save();
    }

    public void IncreaseWage(int masterId, decimal newEarnings)
    {
        var salary = _salariesRepository.GetAll()
            .FirstOrDefault(s => s.IdMaster == masterId);

        if (salary == null)
            throw new ArgumentNullException($"Salary with ID {masterId} not found.");

        if (newEarnings <= 0)
            throw new ArgumentException("New earnings must be greater than 0.");

        salary.Earnings += newEarnings;
        _salariesRepository.Update(salary);
        _salariesRepository.Save();
    }
    public void DecreaseWage(int masterId, decimal Withdrawal)
    {
        var salary = _salariesRepository.GetAll()
            .FirstOrDefault(s => s.IdMaster == masterId);

        if (salary == null)
            throw new ArgumentNullException($"Salary with ID {masterId} not found.");

        if (Withdrawal <= 0)
            throw new ArgumentException("Withdrawal must be greater than 0.");

        if (salary.Earnings - Withdrawal < 0)
            throw new InvalidOperationException("Cannot withdraw more than the current earnings.");

        salary.Earnings -= Withdrawal;
        _salariesRepository.Update(salary);
        _salariesRepository.Save();
    }

    public List<Salary> GetSalaries(List<Master> masters)
    {
        var salaries = new List<Salary>();
        foreach (var master in masters)
        {
            var salary = _salariesRepository.GetAll()
                .FirstOrDefault(s => s.IdMaster == master.Id);
            if (salary != null)
            {
                salaries.Add(salary);
            }
        }
        return salaries;
    }
}