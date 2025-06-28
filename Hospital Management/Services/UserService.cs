using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            user.UserID = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.User.FindAsync(userId);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.User.ToListAsync();
        }
        public async Task<User?> GetUserByRole(string role)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Role == role && u.ActiveFlag);
        }
       public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            var doctors = await _context.User
                .Where(u => u.Role == "Doctor" && u.ActiveFlag)
                .Select(u => new Doctor
                {
                    DoctorID = u.UserID,
                    Name = u.Name,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
            return doctors;
        }

        public async Task<bool> AddDoctorAsync(string name, string email,int age, string password)
        {
            try
            {
                var existingUser = await _context.User
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (existingUser != null)
                    return false; 

                var doctor = new User
                {
                    UserID = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    Password = password, 
                    Role = "Doctor",
                    Age = age,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ActiveFlag = true
                };

                _context.User.Add(doctor);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                user.UpdatedAt = DateTime.UtcNow;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<IEnumerable<User>> GetDoctorsAsync()
        {
            return await _context.User
                .Where(u => u.Role == "Doctor" && u.ActiveFlag)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetPatientsAsync()
        {
            return await _context.User
                .Where(u => u.Role == "Patient" && u.ActiveFlag)
                .ToListAsync();
        } 

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserID == userId && u.ActiveFlag);

                if (user == null)
                    return false;

                user.ActiveFlag = false;
                user.UpdatedAt = DateTime.UtcNow;

                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.User
                .FirstOrDefaultAsync(u => u.Email == email && u.ActiveFlag);
        }

        public async Task<bool> RegisterUserAsync(RegisterDTO dto)
        {
            try
            {
                var user = new User
                {
                    UserID = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = dto.Password, 
                    Role = "Patient",
                    Age = dto.Age,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ActiveFlag = true
                };
                _context.User.Add(user);
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
