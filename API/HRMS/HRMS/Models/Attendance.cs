using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public partial class Attendance
    {
        public string EmpId { get; set; } = null!;
        public DateTime Day { get; set; }
        public TimeSpan AttendingTime { get; set; }
        public TimeSpan LeavingTime { get; set; }

        public virtual Employee Emp { get; set; } = null!;
    }
}
