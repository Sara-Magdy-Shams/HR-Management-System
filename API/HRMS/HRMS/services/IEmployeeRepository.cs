using HRMS.Models;
using HRMS.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.services
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddEmployee(EmployeeViewModel employee);
        Task<Employee> UpdateEmployee(Employee employee );
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> GetEmployee(string employeeId);
        Task DeleteEmployee(string employeeId);

    }
}
