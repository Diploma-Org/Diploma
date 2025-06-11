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
    private readonly IClientService _clientService;

    public HomeController(IHomeService homeService, IAppointmentService appointmentService, IClientService clientService)
    {
        _homeService = homeService;
        _appointmentService = appointmentService;
        _clientService = clientService;
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
            MasterServices = _homeService.GetMasterServices(_homeService.GetCurrentMasters(selectedDate)),
            ProvidedServices = _homeService.GetProvidedServices(),
            Appointments = appointmentAllData,
            SelectedDate = selectedDate,
            Clients = _clientService.GetClients()
        };
        return View(model);
    }
    public IActionResult BookAnAppointment(AppointmentBookingDto model)
    {
        _appointmentService.AddAppointment(model);
        return RedirectToAction("Index", new { date = model.Date });
    }
    public IActionResult EditAppointment(AppointmentBookingDto model)
    {
        _appointmentService.EditAppointment(model);
        return RedirectToAction("Index", new { date = model.Date });
    }

    public IActionResult DeleteAppointment(AppointmentBookingDto model)
    {
        _appointmentService.DeleteAppointment(model);
        return RedirectToAction("Index", new { date = model.Date });
    }
    [HttpGet]
    public IActionResult SearchClients(string query)
    {
        var matches = _clientService.GetClients()
            .Where(c =>
                c.Name.Contains(query) ||
                c.Surname.Contains(query) ||
                c.PhoneNumber.Contains(query))
            .Select(c => new
            {
                id = c.Id,
                name = c.Name,
                surname = c.Surname,
                phone = c.PhoneNumber
            })
            .Take(10)
            .ToList();

        return Json(matches);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

