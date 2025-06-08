using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using BusinessLogic.DTOs;

namespace WebApp.Controllers
{
    public class SalariesController : Controller
    {
        private readonly IMastersService _mastersService;
        private readonly ISalaryService _salaryService;
        private readonly IAppointmentService _appointmentService;
        private readonly IProvidedServicesService _providedServicesService;
        public SalariesController(
            IMastersService mastersService,
            ISalaryService salaryService,
            IAppointmentService appointmentService,
            IProvidedServicesService providedServicesService)
        {
            _salaryService = salaryService;
            _mastersService = mastersService;
            _appointmentService = appointmentService;
            _providedServicesService = providedServicesService;
        }
        public IActionResult Index()
        {
            var model = new SalaryIndexViewModel(
                _salaryService.GetSalaries(_mastersService.GetMasters()),
                _mastersService.GetMasters());
            return View(model);
        }
        public IActionResult DecreaseWage(int MasterId, float Withdrawal)
        {
            _salaryService.DecreaseWage(MasterId, Withdrawal);
            return RedirectToAction("Index");
        }
        public IActionResult DailyWage(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;
            var appointments = _appointmentService.GetAppointmentsByDate(selectedDate);
            var AppointmentsWithWages = new List<AppointmentWithWage>();
            foreach (var appointment in appointments)
            {
                if (appointment.IsPaid)
                {
                    AppointmentsWithWages.Add(
                        new AppointmentWithWage(appointment,
                        _mastersService.GetMasterById(appointment.IdMaster),
                        _providedServicesService.GetProvidedServiceById(appointment.IdProvidedService)));
                }
            }
            var DailyWageViewModel = new DailyWageViewModel
            {
                AppointmentsWithWages = AppointmentsWithWages,
                DailyWage = _salaryService.GetDailyWage(selectedDate),
                Masters = _mastersService.GetMasters()
            };
            return View(DailyWageViewModel);
        }
    }
}