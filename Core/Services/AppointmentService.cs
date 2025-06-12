using BusinessLogic.Interfaces;
using DataAccess.Entities;
using BusinessLogic.DTOs;
using DataAccess.Interfaces;

namespace BusinessLogic.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IRepository<Salary> _salaryRepository;
    private readonly IRepository<ProvidedService> _providedServiceRepository;
    private readonly IRepository<Master> _masterRepository;
    private readonly ISalaryService _salaryService;

    public AppointmentService(IRepository<Appointment> appointmentRepository,
                                IRepository<Salary> salaryRepository,
                                IRepository<ProvidedService> providedServiceRepository,
                                IRepository<Master> masterRepository,
                                ISalaryService salaryService)
    {
        _appointmentRepository = appointmentRepository;
        _salaryRepository = salaryRepository;
        _providedServiceRepository = providedServiceRepository;
        _masterRepository = masterRepository;
        _salaryService = salaryService;
    }


    public void AddAppointment(AppointmentBookingDto appointment)
    {
        if (!(appointment.ClientName != null && appointment.ClientPhone != null && appointment.StartTime != null && appointment.EndTime != null))
            throw new ArgumentNullException(nameof(appointment));
        var Appointment = new Appointment
        {
            IdMaster = appointment.MasterId,
            IdProvidedService = appointment.ServiceId,
            StartTime = appointment.Date.Add(DateTime.ParseExact(appointment.StartTime, "HH:mm", null).TimeOfDay),
            EndTime = appointment.Date.Add(DateTime.ParseExact(appointment.EndTime, "HH:mm", null).TimeOfDay),
            VisitorName = appointment.ClientName,
            VisitorPhone = appointment.ClientPhone,
            IsPaid = appointment.IsPaid
        };
        _appointmentRepository.Insert(Appointment);
        _appointmentRepository.Save();
        if (Appointment.IsPaid)
        {
            var salary = _salaryRepository.GetAll()
                .FirstOrDefault(s => s.IdMaster == Appointment.IdMaster);
            var providedService = _providedServiceRepository.GetById(Appointment.IdProvidedService);
            var master = _masterRepository.GetById(Appointment.IdMaster);
            if (salary == null || providedService == null || master == null)
                throw new ArgumentNullException($"Salary for master with ID {Appointment.IdMaster} not found.");
            else
            {
                _salaryService.IncreaseWage(master, salary, providedService);
            }
        }
    }
    public void EditAppointment(AppointmentBookingDto appointment)
    {
        if (!(appointment.ClientName != null && appointment.ClientPhone != null && appointment.StartTime != null && appointment.EndTime != null))
            throw new ArgumentNullException(nameof(appointment));
        var Appointment = new Appointment
        {
            Id = appointment.Id,
            IdMaster = appointment.MasterId,
            IdProvidedService = appointment.ServiceId,
            StartTime = appointment.Date.Add(DateTime.ParseExact(appointment.StartTime, "HH:mm", null).TimeOfDay),
            EndTime = appointment.Date.Add(DateTime.ParseExact(appointment.EndTime, "HH:mm", null).TimeOfDay),
            VisitorName = appointment.ClientName,
            VisitorPhone = appointment.ClientPhone,
            IsPaid = appointment.IsPaid
        };
        _appointmentRepository.Update(Appointment);
        _appointmentRepository.Save();
        
        if (appointment.IsPaidCopy != Appointment.IsPaid)
        {
            var salary = _salaryRepository.GetAll()
                .FirstOrDefault(s => s.IdMaster == Appointment.IdMaster);
            var providedService = _providedServiceRepository.GetById(Appointment.IdProvidedService);
            var master = _masterRepository.GetById(Appointment.IdMaster);
            if (salary == null || providedService == null || master == null)
                throw new ArgumentNullException($"Salary for master with ID {Appointment.IdMaster} not found.");
            else
            {
                if (Appointment.IsPaid)
                    _salaryService.IncreaseWage(master, salary, providedService);
                else
                    _salaryService.DecreaseWage(master, salary, providedService, appointment.Date);
            }
        }
    }
    public void DeleteAppointment(AppointmentBookingDto appointment)
    {
        var Appointment = new Appointment
        {
            Id = appointment.Id
        };
        _appointmentRepository.Delete(Appointment);
        _appointmentRepository.Save();
        if (appointment.IsPaid)
        {
            var salary = _salaryRepository.GetAll()
                .FirstOrDefault(s => s.IdMaster == appointment.MasterId);
            var providedService = _providedServiceRepository.GetById(appointment.ServiceId);
            var master = _masterRepository.GetById(appointment.MasterId);
            if (salary == null || providedService == null || master == null)
                throw new ArgumentNullException($"Salary for master with ID {appointment.MasterId} not found.");
            else
            {
                _salaryService.DecreaseWage(master, salary, providedService, appointment.Date);
            }
        }
    }

    public IEnumerable<Appointment> GetAppointmentsByDate(DateTime selectedDate)
    {
        var appointments = _appointmentRepository.GetAll()
            .Where(a => a.StartTime.Date == selectedDate.Date)
            .ToList();
        return appointments;
    }
}