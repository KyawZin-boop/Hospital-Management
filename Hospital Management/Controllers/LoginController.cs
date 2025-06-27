using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Hospital_Management.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly UserService _userService;
        private readonly AppointmentService _appointmentService;

        public LoginController(LoginService loginService, UserService userService, AppointmentService appointmentService)
        {
            _loginService = loginService;
            _userService = userService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                return role switch
                {
                    "Patient" => RedirectToAction("Index", "Home"),
                    "Doctor" => RedirectToAction("Table", "Home"),
                    "Admin" => RedirectToAction("Index", "Dashboard"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO dto)
        {
            var response = await _loginService.Execute(dto);

            if (!response.Success)
            {
                ViewBag.Error = response.Message;
                return View();
            }

            var user = response.Data as User;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user!.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        
    }
}
