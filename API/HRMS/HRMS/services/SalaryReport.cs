using System.Linq;
using System;
using System.Collections.Generic;
using HRMS.Models;
using HRMS.DataTypes;

namespace HRMS.services
{
    public class SalaryReport : ISalaryReport
    {
        private readonly IEmployeeRepository _EmpRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly IHolidayRepository _HolidayRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        public SalaryReport (IAttendanceRepository attendanceRepo,ISettingsRepository settingsRepo , IHolidayRepository HolidayRepo , IEmployeeRepository EmpRepo)
        {
            _EmpRepo = EmpRepo;
            _attendanceRepo = attendanceRepo;
            _settingsRepo = settingsRepo;
            _HolidayRepo = HolidayRepo;
        }
        public async Task<List<Invoice>> Report(int month, int year)
        {
            List<Invoice> salaries= new List<Invoice>();
            foreach (Employee emp in (await _EmpRepo.GetEmployees()).ToList())
            {
                var hourSalary = (double)emp.Salary / (await _HolidayRepo.MonthWorkingDays(month, year)).Count() / ((TimeSpan?)(emp.LeavingTime - emp.ArrivalTime)).Value.TotalHours;
                var extraValue = (emp.Extrahour != null) ? emp.Extrahour : (await _settingsRepo.GetSetting()).ExtraHour;
                var penaltyValue = (emp.Penaltyhour != null) ? emp.Penaltyhour : (await _settingsRepo.GetSetting()).PenaltyHour;

                List<Attendance> attendances = await SingleEmployeeAttendance(month, year, emp.Id);
                Invoice inv =new Invoice(
                    Id : emp.Id,
                    Name : emp.FullName,
                    BasicSalary : (double)emp.Salary,
                    arrivalTime : emp.ArrivalTime,
                    LeavingTime : emp.LeavingTime,
                    extraValueInHours : (int)extraValue *
                    (attendances.Sum(att => Math.Max(0, ((TimeSpan?)(emp.ArrivalTime - att.AttendingTime)).Value.TotalHours))
                    + attendances.Sum(att => Math.Max(0, ((TimeSpan?)(att.LeavingTime - emp.LeavingTime)).Value.TotalHours))),
                    
                    penaltyValueInHours : (int)penaltyValue *
                    (attendances.Sum(att => Math.Max(0, ((TimeSpan?)(att.AttendingTime - emp.ArrivalTime)).Value.TotalHours))
                    + attendances.Sum(att => Math.Max(0, ((TimeSpan?)(emp.LeavingTime - att.LeavingTime)).Value.TotalHours)))
                );
                inv.ExtraPenaliryInPounds(hourSalary);
                inv.SetnAbsance((await empAbsancedays(month, year, emp.Id)).Count());
                inv.CalcSalary((await _HolidayRepo.MonthWorkingDays(month, year)).Count());
                salaries.Add(inv);
            }
            return salaries;
        }
        public async Task<SalaryDetails> SingleEmployeeReport(int month, int year, string empId)
        {
            SalaryDetails SR = new SalaryDetails(
                 await empAbsancedays(month, year, empId), 
                 await SingleEmployeeAttendance(month, year, empId)
                );
            return SR;
        }
        private async Task<List<Attendance>> SingleEmployeeAttendance(int month, int year, string empId)
        {
            return await _attendanceRepo.GetAttendanceRange(empId, new DateTime(year, month, 1), new DateTime(year, month, 1).AddDays(DateTime.DaysInMonth(year, month) - 1));
        }
        private async Task<List<DateTime>> empAbsancedays(int month, int year, string empId)
        {
            List<DateTime> officialWorkingDays = await _HolidayRepo.MonthWorkingDays(month, year);
            List<DateTime> empWorkingDays = (await SingleEmployeeAttendance(month, year, empId)).Select(em => em.Day).ToList();

            return officialWorkingDays.Except(empWorkingDays).ToList();
        }
    }
}
