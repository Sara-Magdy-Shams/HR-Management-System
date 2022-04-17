using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels
{
    public class AttendanceViewModel
    {
        [Required]
        public string EmpId { get; set; } = null!;
        [Required]
        public DateTime Day { get; set; }
        [Required]
        public TimeSpan AttendingTime { get; set; }
        [Required]
        public TimeSpan LeavingTime { get; set; }
    }
}
