using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
namespace BusinessLogic.Services;

public class SalaryService : ISalaryService
{
    private readonly IRepository<Master> _mastersRepository;
    private readonly IRepository<Salary> _salariesRepository;

    public SalaryService(IRepository<Master> mastersRepository,
                         IRepository<Salary> salariesRepository)
    {
        _mastersRepository = mastersRepository;
        _salariesRepository = salariesRepository;
    }

    public void AddSalary(int masterId)
    {
        var master = _mastersRepository.GetById(masterId);
        if (master == null)
            throw new ArgumentNullException($"Master with ID {masterId} not found.");

        var salary = new Salary
        {
            IdMaster = masterId,
            Earnings = 0,
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

    public void IncreaseWage(Master master, Salary salary, ProvidedService providedService)
    {
        if (salary == null || providedService == null || master == null)
            throw new InvalidDataException("Problen during IncreasingWage.");
        else
        {
            float mult = master.WagePercent / 100;
            salary.Earnings += providedService.Price * mult;
            _salariesRepository.Update(salary);
            _salariesRepository.Save();            
        }
    }
    public void DecreaseWage(Master master, Salary salary, ProvidedService providedService, DateTime date)
    {
        if (salary == null || providedService == null || master == null)
            throw new InvalidDataException("Problen during DecreasingWage.");
        else
        {
            float mult = master.WagePercent / 100;
            salary.Earnings -= providedService.Price * mult;
            _salariesRepository.Update(salary);
            _salariesRepository.Save();
        }
    }
    public void DecreaseWage(int masterId, float Withdrawal)
    {
        var salary = _salariesRepository.GetAll()
            .FirstOrDefault(s => s.IdMaster == masterId);

        if (salary == null)
            throw new ArgumentNullException($"Salary with ID {masterId} not found.");

        if (Withdrawal <= 0)
            throw new ArgumentException("Withdrawal must be greater than 0.");

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