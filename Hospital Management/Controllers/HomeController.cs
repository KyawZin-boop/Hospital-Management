using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Hospital_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Hospital_Management.Services;
using System.Security.Claims;

namespace Hospital_Management.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserService _userService;
    private readonly AppointmentService _appointmentService;

    public HomeController(ILogger<HomeController> logger, UserService userService, AppointmentService appointmentService)
    {
        _logger = logger;
        _userService = userService;
        _appointmentService = appointmentService;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Patient"))
        {
            return await PatientDashboard(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }
        else if (User.IsInRole("Doctor"))
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.Name));
            return View("Table", appointments);
        }
        else
        {
            var model = await _appointmentService.Dashboard();
            return RedirectToAction("Index", "Dashboard", model);
        }
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
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

    [HttpGet]
    public async Task<IActionResult> PatientDashboard(Guid patientId)
    {
        var user = await _userService.GetUserByIdAsync(patientId);
        if (user == null || user.Role != "Patient")
        {
            return NotFound("Patient not found.");
        }

        var doctorResponse = await _userService.GetAllDoctors();
        var appointments = await _appointmentService.GetAppointmentsAsync(user.UserID, user.Role);

        var model = new PatientDashboardViewModel
        {
            User = user,
            Doctors = doctorResponse ?? new List<Doctor>(),
            Appointments = appointments ?? new List<AppointmentDTO>()
        };

        return View("User", model);
    }
}
