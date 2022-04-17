using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.services
{
    public class GroupRepository:IGroupRepository
    {
        private readonly HRMSContext _context;

        public GroupRepository(HRMSContext context)
        {
            _context = context;
        }

        public async Task<Group> AddGroup(Group group)
        {
            _context.Groups.Add(@group);
            try
            {
                await _context.SaveChangesAsync();
                return group;
            }
            catch (DbUpdateException)
            {
                if (GroupExists(@group.Id))
                {
                    throw new Exception ("Conflict");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteGroup(byte Id)
        {
            var @group = await _context.Groups.FindAsync(Id);
            if (@group == null)
            {
                throw new Exception("NotFound");
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
        }

        public async Task<Group> GetGroupById(byte Id)
        {
            var @group = await _context.Groups.FindAsync(Id);

            if (@group == null)
            {
                throw new Exception ("Group Not Found");
            }

            return @group; 
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group> UpdateGroup(Group group)
        {
            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return group;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(group.Id))
                {
                    throw new Exception("Group Not Found");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool GroupExists(byte id)
        {
            return _context.Groups.Any(g => g.Id == id);
        }
    }
}
