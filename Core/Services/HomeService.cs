using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace BusinessLogic.Services;

public class HomeService : IHomeService
{
    private readonly IRepository<Appoinment> _appointmentRepository;
    private readonly IRepository<Master> _masterRepository;
    private readonly IRepository<ProvidedService> _providedServiceRepository;

    public HomeService(IRepository<Appoinment> appointmentRepository, 
        IRepository<Master> masterRepository, 
        IRepository<ProvidedService> providedServiceRepository)
    {
        _appointmentRepository = appointmentRepository;
        _masterRepository = masterRepository;
        _providedServiceRepository = providedServiceRepository;
    }

    public IEnumerable<Appoinment> GetAppoinments()
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
    public IEnumerable<Master> GetCurrentMasters(IEnumerable<AppoinmentAllData>appoinmentAllData)
    {
        var masters = _masterRepository.GetAll();
        return masters.Where(m => appoinmentAllData.Any(a => a.IdMaster == m.Id));
    }
    public IEnumerable<AppoinmentAllData> GetAppoinmentsByDate(DateTime date)
    {
        var appoinments = _appointmentRepository.GetAll()
            .Where(a => a.StartTime.Date == date.Date)
            .ToList();

        var providedServices = _providedServiceRepository.GetAll()
            .ToDictionary(s => s.Id, s => s.ServiceName);

        var appoinmentAllDatas = appoinments
            .Where(a => providedServices.ContainsKey(a.IdProvidedService))
            .Select(a => new AppoinmentAllData(a, providedServices[a.IdProvidedService]))
            .ToList();

        return appoinmentAllDatas;
    }
}
