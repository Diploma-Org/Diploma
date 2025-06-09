using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using WebApp.ViewModels;
using DataAccess.Entities;

namespace WebApp.Controllers;

public class ProvidedServicesController : Controller
{
    private readonly IProvidedServicesService _providedServicesService;
    public ProvidedServicesController(IProvidedServicesService providedServicesService)
    {
        _providedServicesService = providedServicesService;
    }

    public IActionResult Index(DateTime? date, string? errorMessage)
    {
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ViewData["ErrorMessage"] = errorMessage;
        }

        return View(_providedServicesService.GetProvidedServices());
    }
    public IActionResult AddProvidedService(ProvidedService providedService)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _providedServicesService.AddProvidedService(providedService);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return RedirectToAction("Index", new { errorMessage = "Failed to add provided service." });
    }
    public IActionResult DeleteProvidedService(int id)
    {
        string? errorMessage = null;
        try
        {
            _providedServicesService.DeleteProvidedService(id);
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }

        return RedirectToAction("Index", new { date = DateTime.Today, errorMessage });
    }
    public IActionResult UpdateProvidedService(ProvidedService providedService)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _providedServicesService.UpdateProvidedService(providedService);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return RedirectToAction("Index", new { errorMessage = "Failed to edit provided service." });
    }
}