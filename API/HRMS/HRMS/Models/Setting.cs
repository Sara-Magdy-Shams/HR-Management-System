using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public partial class Setting
    {
        public string WeekEnd1 { get; set; } = null!;
        public string? WeekEnd2 { get; set; }
        public byte PenaltyHour { get; set; }
        public byte ExtraHour { get; set; }
    }
}
