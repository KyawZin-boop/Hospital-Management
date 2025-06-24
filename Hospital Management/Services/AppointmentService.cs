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
            return await _context.Appointment.ToListAsync();
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsAsync(Guid userId, string role)
        {
            IQueryable<Appointment> query = _context.Appointment.Where(a => a.ActiveFlag);

            if (role == "Doctor")
            {
                query = query.Where(a => a.DoctorID == userId);
            }
            else if (role == "Patient")
            {
                query = query.Where(a => a.PatientID == userId);
            }

            var result = await (
                from appt in query
                join patient in _context.User on appt.PatientID equals patient.UserID
                join doctor in _context.User on appt.DoctorID equals doctor.UserID
                select new AppointmentDTO
                {
                    AppointmentID = appt.AppointmentID,
                    patientName = patient.Name,
                    doctorName = doctor.Name,
                    AppointmentDate = appt.AppointmentDate,
                    CreatedAt = appt.CreatedAt,
                    UpdatedAt = appt.UpdatedAt,
                    ActiveFlag = appt.ActiveFlag
                }
            ).ToListAsync();

            return result;
        }


        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            appointment.AppointmentID = Guid.NewGuid();
            appointment.CreatedAt = DateTime.UtcNow;
            _context.Appointment.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}
