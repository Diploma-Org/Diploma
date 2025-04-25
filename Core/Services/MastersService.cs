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
            throw new ArgumentNullException(nameof(master));

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
    public void RemoveWorkingMaster(int id)
    {
        var workingMaster = _workingMastersRepository.GetById(id);
        if (workingMaster == null)
            throw new ArgumentNullException(nameof(workingMaster));
        _workingMastersRepository.Delete(workingMaster);
        _workingMastersRepository.Save();
    }
    public void RemoveMaster(int masterId)
    {
        var master = _mastersRepository.GetById(masterId);
        if (master == null)
            throw new ArgumentNullException(nameof(master));
        _mastersRepository.Delete(master);
        _mastersRepository.Save();
    }
    public void AddMaster(Master master)
    {
        if (master == null)
            throw new ArgumentNullException(nameof(master));
        _mastersRepository.Insert(master);
        _mastersRepository.Save();
    }
}