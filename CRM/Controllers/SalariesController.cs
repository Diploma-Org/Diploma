using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class SalariesController : Controller
    {
        private readonly IMastersService _mastersService;
        private readonly ISalaryService _salaryService;
        public SalariesController(
            IMastersService mastersService,
            ISalaryService salaryService)
        {
            _salaryService = salaryService;
            _mastersService = mastersService;
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
    }
}