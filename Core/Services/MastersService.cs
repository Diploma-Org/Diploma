using System.ComponentModel;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
namespace BusinessLogic.Services;

public class MastersService : IMastersService
{
    private readonly IRepository<WorkingMaster> _workingMastersRepository;
    private readonly IRepository<Master> _mastersRepository;
    private readonly ISalaryService _salaryService;
    private readonly IMasterServicesService _masterServicesService;

    public MastersService(IRepository<WorkingMaster> workingMastersRepository,
        IRepository<Master> mastersRepository,
        ISalaryService salaryService,
        IMasterServicesService masterServicesService)
    {
        _mastersRepository = mastersRepository;
        _workingMastersRepository = workingMastersRepository;
        _salaryService = salaryService;
        _masterServicesService = masterServicesService;
    }
    public List<Master> GetMasters()
    {
        return _mastersRepository.GetAll().ToList();
    }
    public List<WorkingMaster> GetWorkingMasters()
    {
        return _workingMastersRepository.GetAll().ToList();
    }
    public List<WorkingMaster> GetMastersByDate(DateTime date)
    {
        var workingMasters = _workingMastersRepository.GetAll()
             .Where(a => a.Date == date.Date).ToList();
        return workingMasters;
    }
    public void AddWorkingMaster(int masterId, DateTime date)
    {
        var master = _mastersRepository.GetById(masterId);

        if (master == null)
            throw new ArgumentNullException($"Working master with ID {masterId} not found.");

        var workingMaster = new WorkingMaster
        {
            IdMaster = masterId,
            Date = date
        };

        if (_workingMastersRepository.GetAll().Any(wm => wm.IdMaster == masterId && wm.Date == date))
            throw new InvalidOperationException("This master is already assigned to this date.");

        _workingMastersRepository.Insert(workingMaster);
        _workingMastersRepository.Save();
    }
    public void RemoveWorkingMaster(int masterId)
    {
        var workingMaster = _workingMastersRepository.GetById(masterId);
        if (workingMaster == null)
            throw new ArgumentNullException($"Working master with ID {masterId} not found.");
        _workingMastersRepository.Delete(workingMaster);
        _workingMastersRepository.Save();
    }
    public void RemoveMaster(int masterId)
    {
        var master = _mastersRepository.GetById(masterId);
        if (master == null)
            throw new ArgumentNullException($"Working master with ID {masterId} not found.");
        _mastersRepository.Delete(master);
        _mastersRepository.Save();

        var workingMasters = _workingMastersRepository.GetAll()
            .Where(wm => wm.IdMaster == masterId).ToList();
        foreach (var workingMaster in workingMasters)
        {
            _workingMastersRepository.Delete(workingMaster);
        }
        _workingMastersRepository.Save();
        _masterServicesService.DeleteAllMasterServices(masterId);
    }
    public void AddMaster(string name, string surname, string phone, int wagePercent)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(phone))
            throw new ArgumentException("Name, surname and phone number cannot be empty.");
        var master = new Master
        {
            Name = name,
            Surname = surname,
            PhoneNumber = phone,
            WagePercent = wagePercent
        };
        if (_mastersRepository.GetAll().Any(m => m.PhoneNumber == phone))
            throw new InvalidOperationException("This master already exists.");
        _mastersRepository.Insert(master);
        _mastersRepository.Save();
        _salaryService.AddSalary(_mastersRepository.GetAll()
            .FirstOrDefault(m => m.PhoneNumber == phone)?.Id ?? 0);
    }

    public Master GetMasterById(int masterId)
    {
        var master = _mastersRepository.GetById(masterId);
        if (master == null)
            throw new ArgumentNullException($"Master with ID {masterId} not found.");
        return master;
    }
    public void UpdateMaster(int masterId, string name, string surname, string phone, int wagePercent)
    {
        var master = _mastersRepository.GetById(masterId);
        if (master == null)
            throw new ArgumentNullException($"Master with ID {masterId} not found.");
        master.Name = name;
        master.Surname = surname;
        master.PhoneNumber = phone;
        master.WagePercent = wagePercent;
        _mastersRepository.Update(master);
        _mastersRepository.Save();
    }
}