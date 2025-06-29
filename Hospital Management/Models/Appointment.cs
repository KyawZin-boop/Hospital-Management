﻿using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.Models
{
    public class Appointment
    {
        [Key]
        public Guid AppointmentID { get; set; }
        public Guid PatientID { get; set; }
        public Guid DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; } = "pending";
        public bool ActiveFlag { get; set; } = true;
    }
    public class AppointmentDTO
    {
        public Guid AppointmentID { get; set; }
        public string? patientName { get; set; }
        public string? doctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string status { get; set; } ="pending"; // pending, accept, reject, complete
        public bool ActiveFlag { get; set; } = true;

    }
    // Request models for the new endpoints
    public class UpdateAppointmentRequest
    {
        public Guid AppointmentID { get; set; }
        public Guid PatientID { get; set; }
        public Guid DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public required string status { get; set; }
    }

    public class AddDoctorRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
    }
  
    public class UpdateAppointmentStatusRequest
    {
        public Guid AppointmentId { get; set; }
        public string Status { get; set; }
    }
}
