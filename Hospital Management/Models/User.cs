﻿using System.ComponentModel.DataAnnotations;

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
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool ActiveFlag { get; set; } = true;
    }

    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterDTO
    {
        public required string Name { get; set; }
        public required int Age { get; set; }
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
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }

    }
    public class ResponseList
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public List<object>? Data { get; set; }
    }
    public class PatientDashboardViewModel
    {
        public required User User { get; set; }
        public required IEnumerable<Doctor> Doctors { get; set; }
        public required IEnumerable<AppointmentDTO> Appointments { get; set; }
    }
    public class Barchart
    {
        public DateTime DateTime { get; set; }
        public int Count { get; set; }
    }
    public class Linechart
    {
        public DateTime DateTime { get; set; }
        public int redCount { get; set; }
        public int blueCount { get; set; }
        public int greenCount { get; set; }

    }
    public class AdminResponseModel
    {
        public IEnumerable<AppointmentDTO> Appointments { get; set; }
        public IEnumerable<User> Doctors { get; set; }
        public IEnumerable<User> Patients { get; set; }
        public IEnumerable<Barchart> data { get; set; }
        public IEnumerable<Linechart> data1 { get; set; }

    }
}
