using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hospital_Management.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly UserService _userService;

        public DashboardController(AppointmentService appointmentService, UserService userService)
        {
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _appointmentService.dashboard();
                if (model == null)
                {
                    ViewBag.Error = "Unable to load dashboard data.";
                    return View("Dashboard", new AdminResponseModel());
                }
                ViewBag.ActiveTab = "Home";
                return View("Dashboard", model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading the dashboard.";
                return View("Dashboard", new AdminResponseModel());
            }
        }

        // Patient table view
        [HttpGet]
        public async Task<IActionResult> Patient()
        {
            try
            {
                var model = await _appointmentService.dashboard();
                if (model == null)
                {
                    ViewBag.Error = "Unable to load patient data.";
                    return View("Dashboard", new AdminResponseModel());
                }
                ViewBag.ActiveTab = "Patient";
                return View("Dashboard", model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading patient data.";
                // Log the exception if logging is configured
                return View("Dashboard", new AdminResponseModel());
            }
        }

        // Doctor table view
        [HttpGet]
        public async Task<IActionResult> Doctor()
        {
            try
            {
                var model = await _appointmentService.dashboard();
                if (model == null)
                {
                    ViewBag.Error = "Unable to load doctor data.";
                    return View("Dashboard", new AdminResponseModel());
                }
                ViewBag.ActiveTab = "Doctor";
                return View("Dashboard", model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading doctor data.";
                // Log the exception if logging is configured
                return View("Dashboard", new AdminResponseModel());
            }
        }

        // Appointment table view
        [HttpGet]
        public async Task<IActionResult> Appointment()
        {
            try
            {
                var model = await _appointmentService.dashboard();
                if (model == null)
                {
                    ViewBag.Error = "Unable to load appointment data.";
                    return View("Dashboard", new AdminResponseModel());
                }
                ViewBag.ActiveTab = "Appointment";
                return View("Dashboard", model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading appointment data.";
                // Log the exception if logging is configured
                return View("Dashboard", new AdminResponseModel());
            }
        }
    }
}