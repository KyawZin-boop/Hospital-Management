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
                    patientName = patient.Name ?? "Unknown", // Handle NULL Name
                    doctorName = doctor.Name ?? "Unknown",  // Handle NULL Name
                    AppointmentDate = appt.AppointmentDate,
                    status = appt.Status,
                    CreatedAt = appt.CreatedAt,
                    UpdatedAt = appt.UpdatedAt,
                    ActiveFlag = appt.ActiveFlag
                }
            ).ToListAsync();

            return result;
        }

        public async Task<AdminResponseModel> Dashboard()
        {
            DateTime today = DateTime.Today;
            DateTime sevenDaysAgo = today.AddDays(-6);

            var appointmentsQuery = _context.Appointment
                .Where(a => a.ActiveFlag && a.CreatedAt.Date >= sevenDaysAgo && a.CreatedAt.Date <= today);

            var appointments = await (
                from appt in appointmentsQuery
                join patient in _context.User on appt.PatientID equals patient.UserID
                join doctor in _context.User on appt.DoctorID equals doctor.UserID
                select new AppointmentDTO
                {
                    AppointmentID = appt.AppointmentID,
                    patientName = patient.Name ?? "Unknown",
                    doctorName = doctor.Name ?? "Unknown",
                    AppointmentDate = appt.AppointmentDate,
                    status = appt.Status,
                    CreatedAt = appt.CreatedAt,
                    UpdatedAt = appt.UpdatedAt,
                    ActiveFlag = appt.ActiveFlag
                }
            ).ToListAsync();

            var doctors = await _context.User
                .Where(u => u.Role == "Doctor" && u.ActiveFlag)
                .ToListAsync();

            var patients = await _context.User
                .Where(u => u.Role == "Patient" && u.ActiveFlag)
                .ToListAsync();

            var barchartData = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-i))
                .OrderBy(date => date)
                .Select(date => new Barchart
                {
                    DateTime = date,
                    Count = appointments.Count(a => a.CreatedAt.Date == date)
                })
                .ToList();

            var linechartData = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-i))
                .OrderBy(date => date)
                .Select(date => new Linechart
                {
                    DateTime = date,
                    redCount = appointments.Count(a => a.CreatedAt.Date == date && a.status.ToLower() == "rejected"),
                    blueCount = appointments.Count(a => a.CreatedAt.Date == date && a.status.ToLower() == "completed"),
                    greenCount = appointments.Count(a => a.CreatedAt.Date == date && a.status.ToLower() == "accepted")
                })
                .ToList();

            return new AdminResponseModel
            {
                Appointments = appointments,
                Doctors = doctors,
                Patients = patients,
                data = barchartData,
                data1 = linechartData
            };
        }
        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            appointment.AppointmentID = Guid.NewGuid();
            appointment.Status = "pending";
            appointment.CreatedAt = DateTime.UtcNow;
            _context.Appointment.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId)
        {
            return await _context.Appointment
                .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId && a.ActiveFlag);
        }

        public async Task<bool> UpdateAppointmentAsync(Guid appointmentId, Guid patientId, Guid doctorId, DateTime appointmentDate , string status)
        {
            try
            {
                var appointment = await _context.Appointment
                    .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId && a.ActiveFlag);

                if (appointment == null)
                    return false;

                appointment.PatientID = patientId;
                appointment.DoctorID = doctorId;
                appointment.AppointmentDate = appointmentDate;
                appointment.UpdatedAt = DateTime.UtcNow;
                appointment.Status = status;

                _context.Appointment.Update(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(Guid appointmentId)
        {
            try
            {
                var appointment = await _context.Appointment
                    .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId && a.ActiveFlag);

                if (appointment == null)
                    return false;

                appointment.ActiveFlag = false;
                appointment.UpdatedAt = DateTime.UtcNow;

                _context.Appointment.Update(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateAppointmentStatusAsync(Guid appointmentId, string status)
        {
            try
            {
                var appointment = await _context.Appointment
                    .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId && a.ActiveFlag);

                if (appointment == null)
                    return false;
                appointment.Status = status;
                appointment.UpdatedAt = DateTime.UtcNow;

                _context.Appointment.Update(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}