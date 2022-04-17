using HRMS.Models;

namespace HRMS.services
{
    public interface IHolidayRepository
    {
        Task<Hliday> AddHoliday(Hliday holiday);
        Task<IEnumerable<Hliday>> GetHolidays();
        Task<Hliday?> GetHolidayByName(string name);
        Task<Hliday?> GetHolidayByDate(DateTime Date);
        Task<Hliday> UpdateHoliday(string name, DateTime date, Hliday holiday);
        Task DeleteHoliday(string holidayName);
        Task<List<DateTime>> MonthWorkingDays(int month, int year);
        bool HolidayExists(string? name, DateTime date);
    }
}
