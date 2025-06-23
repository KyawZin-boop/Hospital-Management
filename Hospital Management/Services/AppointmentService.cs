using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByPatientIdAsync(Guid patientID)
        {
            return await _context.Appointments.Where(x => x.PatientID == patientID).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByDoctorIdAsync(Guid doctorID)
        {
            return await _context.Appointments.Where(x => x.DoctorID == doctorID).ToListAsync();
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            appointment.AppointmentID = Guid.NewGuid();
            appointment.CreatedAt = DateTime.UtcNow;
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}
