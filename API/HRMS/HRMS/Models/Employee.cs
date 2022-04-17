using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Attendances = new HashSet<Attendance>();
            Users = new HashSet<User>();
        }

        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public decimal Salary { get; set; }
        public DateTime DOb { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool Gender { get; set; }
        public string NationalId { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public DateTime ContractDate { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan LeavingTime { get; set; }
        public byte? Extrahour { get; set; }
        public byte? Penaltyhour { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
