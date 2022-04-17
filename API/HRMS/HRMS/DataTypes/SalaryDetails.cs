using HRMS.Models;

namespace HRMS.DataTypes
{
    public class SalaryDetails
    {
        public List<DateTime> AbsanceDays { get; set; } = new List<DateTime>();
        public List<Attendance> DaysDetails { get; set; } = new List<Attendance>();
    public SalaryDetails(
            List<DateTime> absanceDays ,
            List<Attendance> daysDetails
        )
    {
            this.AbsanceDays = absanceDays;
            this.DaysDetails = daysDetails;
    }
    }
}
