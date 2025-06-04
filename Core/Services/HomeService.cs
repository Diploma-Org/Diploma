using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace BusinessLogic.Services;

public class HomeService : IHomeService
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IRepository<Master> _masterRepository;
    private readonly IRepository<ProvidedService> _providedServiceRepository;
    private readonly IRepository<WorkingMaster> _workingMasterRepository;
    

    public HomeService(IRepository<Appointment> appointmentRepository, 
        IRepository<Master> masterRepository, 
        IRepository<ProvidedService> providedServiceRepository,
        IRepository<WorkingMaster> workingMasterRepository)
    {
        _appointmentRepository = appointmentRepository;
        _masterRepository = masterRepository;
        _providedServiceRepository = providedServiceRepository;
        _workingMasterRepository = workingMasterRepository;
    }

    public IEnumerable<Appointment> GetAppoinments()
    {
        return _appointmentRepository.GetAll();
    }
    public IEnumerable<Master> GetMasters()
    {
        return _masterRepository.GetAll();
    }
    public IEnumerable<ProvidedService> GetProvidedServices()
    {
        return _providedServiceRepository.GetAll();
    }
    public IEnumerable<Master> GetCurrentMasters(DateTime date)
    {
        var masters = _masterRepository.GetAll();
        return masters
            .Where(m => _workingMasterRepository.GetAll()
                .Any(wm => wm.IdMaster == m.Id && wm.Date.Day == date.Day && wm.Date.Month == date.Month && wm.Date.Year == date.Year))
            .ToList();
    }
    public IEnumerable<AppointmentAllData> GetAppoinmentsByDate(DateTime date)
    {
        var appoinments = _appointmentRepository.GetAll()
            .Where(a => a.StartTime.Date == date.Date)
            .ToList();

        var providedServices = _providedServiceRepository.GetAll()
            .ToDictionary(s => s.Id, s => s.ServiceName);

        var appoinmentAllDatas = appoinments
            .Where(a => providedServices.ContainsKey(a.IdProvidedService))
            .Select(a => new AppointmentAllData(a, providedServices[a.IdProvidedService]))
            .ToList();

        return appoinmentAllDatas;
    }

    public void ChangeStatus(int id, bool status)
    {
        var appointment = _appointmentRepository.GetById(id);
        if (appointment != null)
        {
            appointment.IsPaid = status;
            _appointmentRepository.Update(appointment);
            _appointmentRepository.Save();
        }
        else
        {
            throw new ArgumentException("Appointment not found");
        }
    }
}
