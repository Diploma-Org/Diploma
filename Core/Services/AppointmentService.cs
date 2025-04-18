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

    public void AddAppointment(AppointmentBookingDTO model)
    {
        var appointment = new Appointment
        {
            IdMaster = model.MasterId,
            IdProvidedService = model.ServiceId,
            StartTime = model.Date.Add(DateTime.ParseExact(model.StartTime, "HH:mm", null).TimeOfDay),
            EndTime = model.Date.Add(DateTime.ParseExact(model.EndTime, "HH:mm", null).TimeOfDay),
            VisitorName = model.ClientName,
            VisitorPhone = model.ClientPhone
        };
        _appointmentRepository.Insert(appointment);
        _appointmentRepository.Save();
    }
}