using HRMS.ViewModels;
using HRMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace HRMS.services
{
    public class LogIn : ILogger
    {
        private readonly HRMSContext _context;
        public IConfiguration _configuration;
        public LogIn(HRMSContext context , IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }
        public async Task<string> getToken(LogInViewModel usermodel)
        {
            
            if (usermodel.Password != usermodel.ConfirmPassword)
                throw new Exception("The Confirmed password does not match the password");
            User? user = await _context.Users.Include("Grp").FirstOrDefaultAsync(u => u.Email == usermodel.Email && u.Password == usermodel.Password);
            if(user == null)
                throw new Exception("User Not Found : Incorrect Email or password");

            var claims = new[] {
                        new Claim(ClaimTypes.Email, user.Email ),
                        new Claim("id", user.EmpId.ToString()),
                        new Claim ("CreateAttendance", user.Grp.CreateAttendance.ToString()),
                        new Claim ("CreateEmp", user.Grp.CreateEmp.ToString()),
                        new Claim ("CreateSettings", user.Grp.CreateSettings.ToString()),
                        new Claim ("ReadAttendance", user.Grp.ReadAttendance.ToString()),
                        new Claim ("ReadEmp", user.Grp.ReadEmp.ToString()),
                        new Claim ("ReadSettings", user.Grp.ReadSettings.ToString()),
                        new Claim ("ReadReport", user.Grp.ReadReport.ToString()),
                        new Claim ("UpdateAttendance", user.Grp.UpdateAttendance.ToString()),
                        new Claim ("UpdateEmp", user.Grp.UpdateEmp.ToString()),
                        new Claim ("UpdateSettings", user.Grp.UpdateSettings.ToString()),
                        new Claim ("DeleteAttendance", user.Grp.DeleteAttendance.ToString()),
                        new Claim ("DeleteEmp", user.Grp.DeleteEmp.ToString()),
                        new Claim ("DeleteSettings", user.Grp.DeleteSettings.ToString()),
                        new Claim ("HRrole", user.Grp.GroupName.ToString()),
                    };
            //key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //signin
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //token
            var token = new JwtSecurityToken(
                        "", "",
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);
            string Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }
    }
}
