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
    public AppoinmentAllData ToAppoinmentAllData(Appoinment appoinment, string ServiceName)
    {
        AppoinmentAllData appoinmentAllData = new(appoinment, ServiceName);
        return appoinmentAllData;
    }
    public IEnumerable<AppoinmentAllData>? GetAppoinmentsAllData()
    {
        var appoinments = _appointmentRepository.GetAll();
        var ProvidedServices = _providedServiceRepository.GetAll();
        var appoinmentAllDatas = new List<AppoinmentAllData>();
        foreach (var appoinment in appoinments)
        {
            var serviceName = ProvidedServices.FirstOrDefault(x => x.Id == appoinment.IdProvidedService)?.ServiceName;
            if (serviceName != null)
            {
                appoinmentAllDatas.Add(new AppoinmentAllData(appoinment, serviceName));
            }
            else
            {
                return null;
            }
        }
        return appoinmentAllDatas;
    }

}
