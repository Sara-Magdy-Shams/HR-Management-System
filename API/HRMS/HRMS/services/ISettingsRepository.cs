using HRMS.Models;

namespace HRMS.services
{
    public interface ISettingsRepository
    {
        Task<Setting> GetSetting();
        Task<Setting> SetSetting(Setting setting);
      
    }
}
