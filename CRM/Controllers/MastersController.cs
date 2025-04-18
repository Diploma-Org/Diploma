using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MastersController : Controller
    {
        private readonly IMastersService _mastersService;
        public MastersController(IMastersService mastersService)
        {
            _mastersService = mastersService;
        }
        public IActionResult Masters()
        {
            return View();
        }
    }
}