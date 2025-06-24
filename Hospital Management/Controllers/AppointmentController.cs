using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.CreateAppointmentAsync(appointment);

                // Redirect to the PatientDashboard in LoginController
                return RedirectToAction("PatientDashboard", "Login", new { patientId = appointment.PatientID });
            }

            return BadRequest("Invalid appointment data.");
        }
    }
}
