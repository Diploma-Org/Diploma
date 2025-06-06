using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
namespace BusinessLogic.Services;

public class SalaryService : ISalaryService
{
    private readonly IRepository<Master> _mastersRepository;
    private readonly IRepository<Salary> _salariesRepository;
    private readonly IRepository<DailyWage> _dailyWagesRepository;

    public SalaryService(IRepository<Master> mastersRepository,
                         IRepository<Salary> salariesRepository,
                         IRepository<DailyWage> dailyWagesRepository)
    {
        _mastersRepository = mastersRepository;
        _salariesRepository = salariesRepository;
        _dailyWagesRepository = dailyWagesRepository;
    }

    public void AddSalary(int masterId, float earnings)
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

    public void IncreaseWage(Master master, Salary salary, ProvidedService providedService, DateTime date)
    {
        if (salary == null || providedService == null || master == null)
            throw new InvalidDataException("Problen during IncreasingWage.");
        else
        {
            float mult = master.WagePercent / 100;
            salary.Earnings += providedService.Price * mult;
            _salariesRepository.Update(salary);
            _salariesRepository.Save();

            if (!_dailyWagesRepository.GetAll().Any(dw => dw.Date.Date == date.Date))
            {
                mult = (100 - master.WagePercent) / 100;
                var dailyWage = new DailyWage
                {
                    Date = date,
                    Wage = providedService.Price * mult
                };
                _dailyWagesRepository.Insert(dailyWage);
                _dailyWagesRepository.Save();
            }
            else
            {
                mult = (100 - master.WagePercent) / 100;
                var dailyWage = _dailyWagesRepository.GetAll()
                    .FirstOrDefault(dw => dw.Date.Date == date.Date);
                if (dailyWage != null)
                {
                    dailyWage.Wage += providedService.Price * mult;
                    _dailyWagesRepository.Update(dailyWage);
                    _dailyWagesRepository.Save();
                }
            }
            
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
            if (!_dailyWagesRepository.GetAll().Any(dw => dw.Date.Date == date.Date))
            {
                mult = (100 - master.WagePercent) / 100;
                var dailyWage = new DailyWage
                {
                    Date = date,
                    Wage = -providedService.Price * mult
                };
                _dailyWagesRepository.Insert(dailyWage);
                _dailyWagesRepository.Save();
            }
            else
            {
                mult = (100 - master.WagePercent) / 100;
                var dailyWage = _dailyWagesRepository.GetAll()
                    .FirstOrDefault(dw => dw.Date.Date == date.Date);
                if (dailyWage != null)
                {
                    dailyWage.Wage -= providedService.Price * mult;
                    _dailyWagesRepository.Update(dailyWage);
                    _dailyWagesRepository.Save();
                }
            }
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
    public DailyWage GetDailyWage(DateTime date)
    {
        var dailyWage = _dailyWagesRepository.GetAll()
            .FirstOrDefault(dw => dw.Date.Date == date.Date);
        if (dailyWage == null)
            throw new KeyNotFoundException($"Daily wage for date {date.ToShortDateString()} not found.");
        return dailyWage;
    }
}