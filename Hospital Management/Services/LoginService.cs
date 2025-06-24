using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Hospital_Management.Services;

public class LoginService
{
    private readonly UserService _userService;
    private readonly AppDbContext _context;
    public LoginService(UserService userService, AppDbContext context)
    {
        _userService = userService;
        _context = context;
    }
    public async Task<Response> Execute(LoginDTO requestModel)
    {
        if(requestModel.Email == null || requestModel.Password == null)
        {
            return new Response
            {
                Message = "Email and Password are required.",
                Success = false,
                Data = null
            };
        }
        var user = await _context.User.AsNoTracking().FirstOrDefaultAsync(x => x.Email == requestModel.Email && x.Password==requestModel.Password);
        if (user == null)
        {
            return new Response
            {
                Message = "Invalid email or password.",
                Success = false,
                Data = null
            };
        }
        
        return new Response
        {
            Message = "Login successful.",
            Success = true,
            Data = new User
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                ActiveFlag = user.ActiveFlag
            }
        };
    }
    public async Task LogoutAsync()
    {
        // Implement logout logic if needed
    }
}
