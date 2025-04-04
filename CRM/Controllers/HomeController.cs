using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using BusinessLogic.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IHomeService _homeService;
    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    public IActionResult Index(DateTime? date)
    {
        var selectedDate = date ?? DateTime.Today;

        var appoinmentAllData = _homeService.GetAppoinmentsByDate(selectedDate);
        if (appoinmentAllData == null)
            return NotFound();

        var model = new HomeIndexViewModel
        {
            Masters = _homeService.GetCurrentMasters(appoinmentAllData),
            Appointments = appoinmentAllData,
            SelectedDate = selectedDate
        };

        return View(model);
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
