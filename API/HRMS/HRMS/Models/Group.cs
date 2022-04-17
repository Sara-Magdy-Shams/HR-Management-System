using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public partial class Group
    {
        public Group()
        {
            Users = new HashSet<User>();
        }

        public byte Id { get; set; }
        public bool UpdateSettings { get; set; }
        public bool DeleteSettings { get; set; }
        public bool CreateSettings { get; set; }
        public bool ReadSettings { get; set; }
        public bool UpdateEmp { get; set; }
        public bool DeleteEmp { get; set; }
        public bool CreateEmp { get; set; }
        public bool ReadEmp { get; set; }
        public bool UpdateAttendance { get; set; }
        public bool DeleteAttendance { get; set; }
        public bool CreateAttendance { get; set; }
        public bool ReadAttendance { get; set; }
        public bool ReadReport { get; set; }
        public string GroupName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
