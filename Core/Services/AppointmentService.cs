using BusinessLogic.Interfaces;
using DataAccess.Entities;
using BusinessLogic.DTOs;
using DataAccess.Interfaces;

namespace BusinessLogic.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointmentRepository;

    public AppointmentService(IRepository<Appointment> appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public void AddAppointment(AppointmentBookingDto appointment)
    {
        if (appointment.IsValid())
            throw new ArgumentNullException(nameof(appointment));
        var Appointment = new Appointment
        {
            IdMaster = appointment.MasterId,
            IdProvidedService = appointment.ServiceId,
            StartTime = appointment.Date.Add(DateTime.ParseExact(appointment.StartTime, "HH:mm", null).TimeOfDay),
            EndTime = appointment.Date.Add(DateTime.ParseExact(appointment.EndTime, "HH:mm", null).TimeOfDay),
            VisitorName = appointment.ClientName,
            VisitorPhone = appointment.ClientPhone
        };
        _appointmentRepository.Insert(Appointment);
        _appointmentRepository.Save();
    }
}