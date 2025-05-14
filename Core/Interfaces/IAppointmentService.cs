using BusinessLogic.DTOs;
namespace BusinessLogic.Interfaces;

public interface IAppointmentService
{
    void AddAppointment(AppointmentBookingDto appointment);
    void DeleteAppointment(AppointmentBookingDto appointment);
    void EditAppointment(AppointmentBookingDto appointment);
}
