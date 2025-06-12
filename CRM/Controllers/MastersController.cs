using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class MastersController : Controller
    {
        private readonly IMastersService _mastersService;
        private readonly IMasterServicesService _masterServicesService;
        public MastersController(IMastersService mastersService, IMasterServicesService masterServicesService)
        {
            _mastersService = mastersService;
            _masterServicesService = masterServicesService;
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
            try { _mastersService.RemoveWorkingMaster(id); }
            catch (Exception ex) { errorMessage = ex.Message; }

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
        public IActionResult AddMaster(string name, string surname, string phone, int wagePercent)
        {
            string? errorMessage = null;
            try
            {
                _mastersService.AddMaster(name, surname, phone, wagePercent);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("Index", new { date = DateTime.Today, errorMessage });
        }
        public IActionResult DisplayMasterServices(int? IdMaster)
        {
            ViewBag.Masters = _mastersService.GetMasters();
            try
            {
                return View(_masterServicesService.GetSevicesToDisplayForMaster(IdMaster));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index", "Home", new { date = DateTime.Today, errorMessage = ViewData["ErrorMessage"] });
        }
        public IActionResult AddMasterServiceFromList(int IdMaster, int IdService)
        {
            string? errorMessage = null;
            try
            {
                _masterServicesService.AddMasterServiceFromList(IdMaster, IdService);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("DisplayMasterServices", new { IdMaster, errorMessage });
        }
        public IActionResult DeleteService(int IdMaster, int IdService)
        {
            string? errorMessage = null;
            try
            {
                _masterServicesService.DeleteService(IdMaster, IdService);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("DisplayMasterServices", new { IdMaster, errorMessage });
        }
        public IActionResult UpdateMaster(int id, string name, string surname, string phoneNumber, int wagePercent)
        {
            string? errorMessage = null;
            try
            {
                _mastersService.UpdateMaster(id, name, surname, phoneNumber, wagePercent);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("Index", new { date = DateTime.Today, errorMessage });
        }
    }
}