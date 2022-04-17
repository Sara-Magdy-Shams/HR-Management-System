using HRMS.Models;

namespace HRMS.services
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetGroups();
        Task<Group> GetGroupById(byte Id);
        Task<Group> UpdateGroup(Group group);
        Task<Group> AddGroup(Group group);
        Task DeleteGroup(byte Id);
    }
}
