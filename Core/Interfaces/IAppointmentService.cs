using BusinessLogic.DTOs;
using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IAppointmentService
{
    void AddAppointment(AppointmentBookingDto appointment);
    void DeleteAppointment(AppointmentBookingDto appointment);
    void EditAppointment(AppointmentBookingDto appointment);
    IEnumerable<Appointment> GetAppointmentsByDate(DateTime selectedDate);
}
