using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.services
{
    public class SettingsRepository :ISettingsRepository
    {
        private readonly HRMSContext _context;

        public SettingsRepository(HRMSContext context)
        {
            _context = context;
        }

        public async Task<Setting> GetSetting()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();

            if (setting == null)
            {
                throw new Exception ("NotFound");
            }
            return setting;
        }

        public async Task<Setting> SetSetting(Setting setting)
        {
            if (!SettingExists())
                _context.Settings.Add(setting);
            else
                _context.Entry(setting).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return await GetSetting();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        private bool SettingExists()
        {
            return _context.Settings.Any();
        }
    }
}
