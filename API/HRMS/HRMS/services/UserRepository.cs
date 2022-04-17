using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.services
{
    public class UserRepository : IUserRepository
    {
        private readonly HRMSContext _context;

        public UserRepository(HRMSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FindAsync(email);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            if (!UserEmailExists(user.Email))
            {
                throw new Exception("User Not Found");
            }
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<User> AddUser(User user)
        {
            if (UserEmployeeExists(user.EmpId))
                throw new Exception("This Employee is allready have account");
            if (UserEmailExists(user.Email))
                throw new Exception("This Email is asigned to another employee");

            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return user; 
        }

        public async Task DeleteUser(string email)
        {
            {
                var user = await _context.Users.FindAsync(email);
                if (user == null)
                {
                    throw new Exception("User Not Found");
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
       
        private bool UserEmailExists(string email)
        {
            return _context.Users.Any(user => user.Email == email);
        }
        private bool UserEmployeeExists(string empId)
        {
            return _context.Users.Any(user => user.EmpId == empId);
        }
    }
}
