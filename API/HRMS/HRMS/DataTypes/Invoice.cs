namespace HRMS.DataTypes
{
    public class Invoice
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double BasicSalary { get; set; }
        public TimeSpan arrivalTime { get; set; }
        public TimeSpan LeavingTime { get; set; }
        public double extraValueInHours { get; set; } = 0;
        public double penaltyValueInHours { get; set; } = 0;
        public double extraInPounds { get; set; }
        public double PenalityInPounds { get; set; }
        public byte nAbsance { get; set; } = 0;
        public double MonthSalary { get; set; }
        public Invoice(
                string Id,
                string Name,
                double BasicSalary,
                TimeSpan arrivalTime,
                TimeSpan LeavingTime,
                double extraValueInHours,
                double penaltyValueInHours
            )
        {
            this.Id = Id;
            this.Name = Name;
            this.BasicSalary = BasicSalary;
            this.extraValueInHours = extraValueInHours;
            this.penaltyValueInHours = penaltyValueInHours;
            this.arrivalTime = arrivalTime;
            this.LeavingTime = LeavingTime;
        }

        public void ExtraPenaliryInPounds(double hourlySalary)
        {
            this.extraInPounds = this.extraValueInHours * hourlySalary;
            this.PenalityInPounds = this.penaltyValueInHours * hourlySalary;
        }

        public void SetnAbsance(int nAbcanceDays)
        {
            this.nAbsance = (byte)nAbcanceDays;
        }

        public void CalcSalary(int nWorkingDays)
        {
            this.MonthSalary = BasicSalary + this.extraInPounds - PenalityInPounds - this.nAbsance * BasicSalary / nWorkingDays;
        }
    }
}
