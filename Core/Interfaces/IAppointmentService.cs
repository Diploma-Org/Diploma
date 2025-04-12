using BusinessLogic.DTOs;
namespace BusinessLogic.Interfaces;

    public interface IAppointmentService
    {
        void AddAppointment(AppointmentBookingDTO appointment);
    }
