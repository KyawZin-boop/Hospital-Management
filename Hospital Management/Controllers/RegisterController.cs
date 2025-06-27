using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly UserService _userService;

        public RegisterController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userExists = await _userService.GetUserByEmailAsync(dto.Email);
            if (userExists != null)
            {
                ViewBag.Error = "Email already registered.";
                return View(dto);
            }

            var result = await _userService.RegisterUserAsync(dto);

            if (!result)
            {
                ViewBag.Error = "Error during registered.";
                return View(dto);
            }
            TempData["Success"] = "Registration successful! Please log in.";
            // Redirect to login after successful registration
            return RedirectToAction("Index", "Login");
        }
    }
}
