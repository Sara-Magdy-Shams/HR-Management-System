using HRMS.Models;
using HRMS.ViewModels;

namespace HRMS.services
{
    public interface IAttendanceRepository
    {
        Task<Attendance> AddAttendance(AttendanceViewModel Attendance);
        Task<Attendance> UpdateAttendance(string employeeId, DateTime Day, Attendance attendance);
        Task DeleteAttendance(string employeeId, DateTime Day);
        Task<IEnumerable<Attendance>> GetAttendances();
        Task<List<Attendance>> GetAttendanceRange(string? employeeId , DateTime StartDay , DateTime EndDay);
    }
}
