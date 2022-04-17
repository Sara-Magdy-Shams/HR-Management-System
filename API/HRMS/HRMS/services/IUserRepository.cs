using HRMS.Models;

namespace HRMS.services
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByEmail(string email);
        Task<User> UpdateUser(User user); 
        Task<User> AddUser(User user);
        Task DeleteUser(string email);
    }
}
