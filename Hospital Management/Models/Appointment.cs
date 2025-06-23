namespace Hospital_Management.Models
{
    public class Appointment
    {
        public Guid AppointmentID { get; set; }
        public Guid PatientID { get; set; }
        public Guid DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool ActiveFlag { get; set; } = true;
    }
}
