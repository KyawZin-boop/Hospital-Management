using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
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

            ViewData["Name"] = user.Name;

            if (user.Role == "Patient")
            {
                return await PatientDashboard(user.UserID);
            }
            else if(user.Role == "Doctor")
            {
                var appointments = await _appointmentService.GetAppointmentsAsync(user.UserID, user.Role);
                return View("Table", appointments);
            }
            else
            {
                var doctors = await _userService.GetAllDoctors();
                var appointments = await _appointmentService.GetAppointmentsAsync(user.UserID, user.Role);
                var model = new AdminResponseModel
                {
                    Doctors = doctors,
                    Appointments = appointments
                };
                return View("Table", model);
            }
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
}
