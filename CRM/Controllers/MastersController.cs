using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class MastersController : Controller
    {
        private readonly IMastersService _mastersService;
        public MastersController(IMastersService mastersService)
        {
            _mastersService = mastersService;
        }
        public IActionResult Index(DateTime? date, string? errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
            var selectedDate = date ?? DateTime.Today;

            var WorkingMasters = _mastersService.GetMastersByDate(selectedDate);
            if (WorkingMasters == null)
                return NotFound();
            var masters = _mastersService.GetMasters();
            var viewModel = new MastersIndexViewModel(masters, WorkingMasters, selectedDate);
            return View(viewModel);
        }
        public IActionResult AssignMasterToDate(int masterId, DateTime date)
        {
            string? errorMessage = null;
            try
            {
                _mastersService.AddWorkingMaster(masterId, date);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("Index", new { date, errorMessage });
        }
        public IActionResult RemoveMasterFromDate(int id)
        {
            string? errorMessage = null;
            try {_mastersService.RemoveWorkingMaster(id);}
            catch (Exception ex){errorMessage = ex.Message;}

            return RedirectToAction("Index", new { date = DateTime.Today, errorMessage });

        }
        public IActionResult DeleteMaster(int id)
        {
            string? errorMessage = null;
            try
            {
                _mastersService.RemoveMaster(id);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("Index", new { date = DateTime.Today, errorMessage });
        }
        public IActionResult AddMaster(string name, string surname, string phone)
        {
            string? errorMessage = null;
            try
            {
                _mastersService.AddMaster(name, surname, phone);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("Index", new { date = DateTime.Today, errorMessage });
        }
    }
}