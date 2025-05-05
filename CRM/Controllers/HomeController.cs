using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IHomeService _homeService;
    private readonly IAppointmentService _appointmentService;
    public HomeController(IHomeService homeService, IAppointmentService appointmentService)
    {
        _homeService = homeService;
        _appointmentService = appointmentService;
    }


    public IActionResult Index(DateTime? date, string? errorMessage)
    {
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ViewData["ErrorMessage"] = errorMessage;
        }
        var selectedDate = date ?? DateTime.Today;

        var appointmentAllData = _homeService.GetAppoinmentsByDate(selectedDate);
        if (appointmentAllData == null)
            return NotFound();


        var model = new HomeIndexViewModel
        {
            Masters = _homeService.GetCurrentMasters(selectedDate),
            ProvidedServices = _homeService.GetProvidedServices(),
            Appointments = appointmentAllData,
            SelectedDate = selectedDate
        };


        return View(model);
    }
    public IActionResult BookAnAppointment(AppointmentBookingDto model)
    {
        _appointmentService.AddAppointment(model);
        return RedirectToAction("Index", new { date = model.Date });
    }


    public IActionResult Privacy()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
