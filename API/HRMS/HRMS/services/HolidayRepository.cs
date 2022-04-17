using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.services
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly HRMSContext _context;
        private readonly ISettingsRepository _settingsRepo;

        public HolidayRepository(HRMSContext context ,ISettingsRepository settingRepo)
        {
            _context = context;
            _settingsRepo = settingRepo;
        }
        public async Task<Hliday> AddHoliday(Hliday holiday)
        {
            if (HolidayExists(holiday.Name, holiday.Date))
            {
                throw new Exception("Record Data Must Be Unique");
            }
            _context.Hlidays.Add(holiday);
            try
            {
                await _context.SaveChangesAsync();
                return holiday;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task DeleteHoliday(string holidayName)
        {
            //throw new NotImplementedException();
            Hliday? hliday = await _context.Hlidays.FirstOrDefaultAsync(holi=>holi.Name==holidayName);
            if (hliday == null)
            {
                throw new Exception("not found");
            }

            _context.Hlidays.Remove(hliday);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Hliday>> GetHolidays()
        {
            return await _context.Hlidays.ToListAsync();
        }
        public async Task<Hliday?> GetHolidayByName(string name)
        {
            if (!_context.Hlidays.Any(holi => holi.Name == name))
            {
                throw new Exception("Name Not Found");
            }
            else
            {
                return await _context.Hlidays.FirstOrDefaultAsync(holiday => holiday.Name == name);
            }
        }
        public async Task<Hliday?> GetHolidayByDate(DateTime Date)
        {
            if (!_context.Hlidays.Any(holi => holi.Date == Date))
            {
                throw new Exception("Aate Not Found");
            }
            else
            {
                return await _context.Hlidays.FirstOrDefaultAsync(holiday => holiday.Date == Date);
            }
        }
        public async Task<Hliday> UpdateHoliday(string name, DateTime date , Hliday holiday)
        {
            if (!_context.Hlidays.Any(holi => holi.Name == name))
            {
                throw new Exception("Name Not found");
            }
            if (!_context.Hlidays.Any(holi => holi.Date == date))
            {
                throw new Exception("Date Not Found");
            }
            
            _context.Entry(holiday).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return holiday;
            }
            catch (DbUpdateConcurrencyException)
            {
                 throw;
            }
        }
        public async Task<List<DateTime>> MonthWorkingDays(int month, int year)
        {
            int range = DateTime.DaysInMonth(year, month);
            List<DateTime> Monthdays = Enumerable.Range(1, range)
                            .Select(day => new DateTime(year, month, day))
                            .ToList();
            List<DateTime> Holidays = (await GetHolidays()).Select(h => h.Date).ToList();
            Setting s = await _settingsRepo.GetSetting();
            List<DateTime> WeekEndDays =
                Monthdays.Where(day => day.ToString("dddd") == s.WeekEnd1
                    || day.ToString("dddd") == s.WeekEnd2
                ).ToList();
            List<DateTime> Vications = Holidays.Union(WeekEndDays).ToList();
            List<DateTime> WorkingDays = Monthdays.Except(Vications).ToList();
            return WorkingDays;
        }
        public bool HolidayExists(string? name, DateTime date)
        {
            return _context.Hlidays.Any(holi => holi.Name == name || holi.Date == date);
        }
    }
}
