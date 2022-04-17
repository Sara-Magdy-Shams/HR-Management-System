using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmployeeValidations
    {
        [MinLength(3)]
        public string FullName { get; set; } = null!;

        [RegularExpression(@"^01[0|1|2|5]{1}[0-9]{8}$",ErrorMessage ="not valid phone number id")]
        public string Phone { get; set; } = null!;

        [RegularExpression(@"^[2|3]\d{13}",ErrorMessage ="not valid national id")]
        public string NationalId { get; set; } = null!;
    }
    [ModelMetadataType(typeof(EmployeeValidations))]
    public partial class Employee
    { }
}
