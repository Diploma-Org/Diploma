using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
namespace BusinessLogic.Services;

public class MastersService : IMastersService
{
    private readonly IRepository<WorkingMaster> _workingMastersRepository;
    private readonly IRepository<Master> _mastersRepository;

    public MastersService(IRepository<WorkingMaster> workingMastersRepository,
        IRepository<Master> mastersRepository)
    {
        _mastersRepository = mastersRepository;
        _workingMastersRepository = workingMastersRepository;
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
    }
    public void AddMaster(string name, string surname, string phone)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(phone))
            throw new ArgumentException("Name, surname and phone number cannot be empty.");
        var master = new Master
        {
            Name = name,
            Surname = surname,
            PhoneNumber = phone
        };
        if (_mastersRepository.GetAll().Any(m => m.PhoneNumber == phone))
            throw new InvalidOperationException("This master already exists.");
        _mastersRepository.Insert(master);
        _mastersRepository.Save();
    }
}