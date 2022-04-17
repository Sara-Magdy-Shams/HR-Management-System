using HRMS.Models;
using HRMS.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HRMS.services
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly HRMSContext _context;
        private readonly ISettingsRepository _settingsrepo;
        private readonly IHolidayRepository _holidayRepo;
        public AttendanceRepository(HRMSContext context, ISettingsRepository settingsrepo, IHolidayRepository holidayRepo) {
            _context = context;
            _settingsrepo = settingsrepo;
            _holidayRepo = holidayRepo;
        }
        public async Task<Attendance> AddAttendance(AttendanceViewModel att)
        {
            Attendance attendance = new Attendance();
            attendance.EmpId = att.EmpId;   
            attendance.Day = att.Day;
            attendance.AttendingTime = att.AttendingTime;
            attendance.LeavingTime = att.LeavingTime;
            if (!_context.Employees.Any(emp => emp.Id == attendance.EmpId))
                throw new Exception("There is no employee with this id");
            if (AttendanceExists(attendance))
                throw new Exception("The Employee has been already signed in that day");
            if (attendance.AttendingTime > attendance.LeavingTime)
                throw new Exception("Not logical sign in and signout time");
            if(await DayOff(attendance))
            {
                throw new Exception("This is a day Off");
            }
            _context.Attendances.Add(attendance);
            try
            {
                await _context.SaveChangesAsync();
                return attendance;
            }
            catch (DbUpdateException)
            {
                throw;
            }
 
        }

        public async Task DeleteAttendance(string employeeId, DateTime Day)
        {
            var attendance = await _context.Attendances.FirstOrDefaultAsync(att => att.Day == Day && att.EmpId == employeeId);
            if (attendance==null)
            {
                throw new Exception("NotFound");
            }
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAttendances()
        {
            return await _context.Attendances.OrderBy(att => att.Day).ToListAsync();
        }

        public async Task<List<Attendance>>GetAttendanceRange(string? employeeId, DateTime StartDay, DateTime EndDay)
        {
            if (employeeId != null && !_context.Employees.Any(emp => emp.Id == employeeId))
            {
                throw new ArgumentException(
                    "there isn't any employee having this id"
                    );
            }
            if (StartDay > EndDay)
                throw new ArgumentException("Not Logical Time Span");
            return (employeeId == null) ? await _context.Attendances
                     .Where(A => A.Day >= StartDay && A.Day <= EndDay)
                     .OrderBy(A => A.Day)
                     .ToListAsync()
                 : await _context.Attendances
                     .Where(A => A.Day >= StartDay && A.Day <= EndDay && A.EmpId == employeeId)
                     .OrderBy(A => A.Day)
                     .ToListAsync();
        }

        public async Task<Attendance> UpdateAttendance(string employeeId, DateTime Day , Attendance attendance )
        {
            _context.Entry(attendance).State = EntityState.Modified;
            if (!AttendanceExists(attendance))
            {
                throw new ArgumentException("NotFound");
            }
            if (attendance.LeavingTime < attendance.AttendingTime)
                throw new ArgumentException("Not logical shift times");
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return attendance;
        }
        private bool AttendanceExists(Attendance attendance)
        {
            return _context.Attendances.Any(att => att.Day == attendance.Day && att.EmpId == attendance.EmpId);
        }

        private async Task<bool> DayOff(Attendance att)
        {
            string day = att.Day.ToString("dddd");
            if (att.Day.ToString("dddd") == (await _settingsrepo.GetSetting()).WeekEnd1 || att.Day.ToString("dddd") == (await _settingsrepo.GetSetting()).WeekEnd2)
                return true;
            if (_holidayRepo.HolidayExists(null, att.Day))
                return true;
            return false;
        }

    }
}
