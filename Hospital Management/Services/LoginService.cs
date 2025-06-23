using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Hospital_Management.Services
{
    public class LoginService
    {
        private readonly UserService _userService;
        private readonly AppDbContext _context;
        public LoginService(UserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }
        public async Task<LoginResponseModel> Execute(LoginRequestModel requestModel)
        {
            LoginResponseModel responseModel = new();

            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == requestModel.Email);
                if (user == null)
                {
                    responseModel.Success = false;
                    responseModel.Message = "User not found.";
                    return responseModel;
                }

                if (user.Status == "N")
                {
                    responseModel.Success = false;
                    responseModel.Message = "User not verified.";
                    return responseModel;
                }

                var passwordHash = DevCode.HashPassword(requestModel.Password);
                if (!string.Equals(passwordHash, user.PasswordHash))
                {
                    responseModel.Success = false;
                    responseModel.Message = "Wrong credentials";
                    return responseModel;
                }

                LoginTokenModel loginTokenModel = new()
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.Phone,
                    ExpireTime = DateTime.Now.AddMinutes(5),
                    Role = user.Role,

                };

                var token = loginTokenModel.ToJson().ToEncrypt();

                responseModel.Success = true;
                responseModel.Message = "Success";
                responseModel.Token = token;
                return responseModel;

            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.ToString();
                return responseModel;
            }
        }
        public async Task LogoutAsync()
        {
            // Implement logout logic if needed
        }
    }
}
