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
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.User.FindAsync(userId);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.User.ToListAsync();
        }
        public async Task<User> GetUserByRole(string role)
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
    }
}
