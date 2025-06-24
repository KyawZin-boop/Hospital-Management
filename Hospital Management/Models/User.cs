using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.Models
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; } 
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool ActiveFlag { get; set; } = true;
    }

    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class Response
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public object? Data { get; set; }
    }
    public class Doctor
    {
        public Guid DoctorID { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

    }
    public class ResponseList
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public List<object> Data { get; set; }
    }
    public class PatientDashboardViewModel
    {
        public required User User { get; set; }
        public required IEnumerable<Doctor> Doctors { get; set; }
        public required IEnumerable<AppointmentDTO> Appointments { get; set; }
    }

    public class AdminResponseModel
    {
        public IEnumerable<AppointmentDTO> Appointments { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }
      

    }
}
