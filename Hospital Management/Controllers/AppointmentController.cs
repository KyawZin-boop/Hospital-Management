using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital_Management.Controllers
{
    [Authorize]
    [Route("api/appointments")] // API route prefix
    [ApiController] // Mark as API controller for JSON responses
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Route("create")] // Route: /api/appointments/create
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.CreateAppointmentAsync(appointment);
                return RedirectToAction("PatientDashboard", "Home", new { patientId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) });
            }

            return BadRequest("Invalid appointment data.");
        }

        [HttpPost]
        [Route("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateAppointmentStatusRequest request)
        {
            if (request == null || request.AppointmentId == Guid.Empty || string.IsNullOrEmpty(request.Status))
            {
                return BadRequest(new { success = false, message = "Invalid request data." });
            }

            if (!User.IsInRole("Doctor"))
            {
                return Unauthorized(new { success = false, message = "Only doctors can update appointment status." });
            }

            string status = request.Status.ToLower() switch
            {
                "accept" => "accepted",
                "reject" => "rejected",
                "complete" => "completed",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(status))
            {
                return BadRequest(new { success = false, message = "Invalid status value." });
            }

            bool success = await _appointmentService.UpdateAppointmentStatusAsync(request.AppointmentId, status);
            if (success)
            {
                return Ok(new { success = true, message = $"Appointment {status} successfully." });
            }

            return BadRequest(new { success = false, message = "Failed to update appointment status." });
        }
    }
}

