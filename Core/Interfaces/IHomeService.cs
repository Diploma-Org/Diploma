using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

    public interface IHomeService
    {
        public IEnumerable<Appoinment> GetAppoinments();
        public IEnumerable<Master> GetMasters();
        public IEnumerable<Master> GetCurrentMasters(IEnumerable<AppoinmentAllData> appoinmentAllData);
        public IEnumerable<ProvidedService> GetProvidedServices();
        public IEnumerable<AppoinmentAllData>? GetAppoinmentsByDate(DateTime date);
    }
