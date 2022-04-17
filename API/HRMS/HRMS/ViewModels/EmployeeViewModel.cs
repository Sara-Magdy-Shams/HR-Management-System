using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels
{
    public class EmployeeViewModel
    {
        [Required]
        [MinLength(3)]
        public string FullName { get; set; } = String.Empty;

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public DateTime DOb { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(11)]
        [RegularExpression(@"^01[0|1|2|5]{1}[0-9]{8}$", ErrorMessage = "not valid phone number id")]
        public string Phone { get; set; } = null!;

        [Required]
        public bool Gender { get; set; }

        [Required]
        [StringLength(14)]
        [RegularExpression(@"^[2|3]\d{13}", ErrorMessage = "not valid national id")]
        public string NationalId { get; set; } = null!;

        [Required]
        public string Nationality { get; set; } = null!;

        [Required]
        public DateTime ContractDate { get; set; }

        [Required]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        public TimeSpan LeavingTime { get; set; }
        public byte? Extrahour { get; set; }
        public byte? Penaltyhour { get; set; }
    }
}
