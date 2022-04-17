using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public partial class User
    {
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte GrpId { get; set; }
        public string EmpId { get; set; } = null!;

        public virtual Employee Emp { get; set; } = null!;
        public virtual Group Grp { get; set; } = null!;
    }
}
