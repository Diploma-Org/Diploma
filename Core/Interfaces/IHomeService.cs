using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

    public interface IHomeService
    {
        public IEnumerable<Appoinment> GetAppoinments();
        public IEnumerable<Master> GetMasters();
        public IEnumerable<ProvidedService> GetProvidedServices();
        public AppoinmentAllData ToAppoinmentAllData(Appoinment appoinment, string ServiceName);
        public IEnumerable<AppoinmentAllData>? GetAppoinmentsAllData();
    }
