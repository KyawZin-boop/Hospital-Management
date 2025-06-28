using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hospital_Management.Controllers
{
    [Authorize(Roles = "Admin")]
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
                var model = await _appointmentService.Dashboard();
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
                var model = await _appointmentService.Dashboard();
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
                return View("Dashboard", new AdminResponseModel());
            }
        }

        // Doctor table view
        [HttpGet]
        public async Task<IActionResult> Doctor()
        {
            try
            {
                var model = await _appointmentService.Dashboard();
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
                return View("Dashboard", new AdminResponseModel());
            }
        }

        // Appointment table view
        [HttpGet]
        public async Task<IActionResult> Appointment()
        {
            try
            {
                var model = await _appointmentService.Dashboard();
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
                return View("Dashboard", new AdminResponseModel());
            }
        }

        // Get appointment for editing
        [HttpGet]
        public async Task<IActionResult> GetAppointment(Guid id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null)
                {
                    return Json(new { success = false, message = "Appointment not found." });
                }

                var doctors = await _userService.GetDoctorsAsync();
                var patients = await _userService.GetPatientsAsync();

                return Json(new
                {
                    success = true,
                    appointment = new
                    {
                        appointmentID = appointment.AppointmentID,
                        patientID = appointment.PatientID,
                        doctorID = appointment.DoctorID,
                        appointmentDate = appointment.AppointmentDate.ToString("yyyy-MM-ddTHH:mm")
                    },
                    doctors = doctors.Select(d => new { value = d.UserID, text = d.Name }),
                    patients = patients.Select(p => new { value = p.UserID, text = p.Name })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error retrieving appointment data." });
            }
        }

        // Update appointment
        [HttpPost]
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data provided." });
                }

                var result = await _appointmentService.UpdateAppointmentAsync(request.AppointmentID, request.PatientID, request.DoctorID, request.AppointmentDate, request.status);

                if (result)
                {
                    return Json(new { success = true, message = "Appointment updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to update appointment." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while updating the appointment." });
            }
        }

        // Delete appointment
        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            try
            {
                var result = await _appointmentService.DeleteAppointmentAsync(id);

                if (result)
                {
                    return Json(new { success = true, message = "Appointment deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete appointment." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the appointment." });
            }
        }

        // Add new doctor
        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] AddDoctorRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data provided." });
                }

                var result = await _userService.AddDoctorAsync(request.Name, request.Email,request.Age, request.Password);

                if (result)
                {
                    return Json(new { success = true, message = "Doctor added successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to add doctor." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while adding the doctor." });
            }
        }
    }


}