using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IHomeService
{
    IEnumerable<Appointment> GetAppoinments();
    IEnumerable<Master> GetMasters();
    IEnumerable<Master> GetCurrentMasters(DateTime date);
    IEnumerable<ProvidedService> GetProvidedServices();
    IEnumerable<AppointmentAllData>? GetAppoinmentsByDate(DateTime date);
}