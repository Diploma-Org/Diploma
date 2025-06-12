using System.Threading.Tasks;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly IAdminUsersService _adminUsersService;

        public AdminUsersController(IAdminUsersService adminUsersService)
        {
            _adminUsersService = adminUsersService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _adminUsersService.GetAllUsersAsync();
            return View(users);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var result = await _adminUsersService.DeleteUserAsync(id);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", result.Errors);
            }
        }
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            var result = await _adminUsersService.CreateUserAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", result.Errors);
            }
        }
    }
}
