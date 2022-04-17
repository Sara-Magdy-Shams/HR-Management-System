using HRMS.Models;
using HRMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HRMS.services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSContext _context ;
        
        public EmployeeRepository(HRMSContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployee(EmployeeViewModel employee)
        {
            Employee emp = new Employee();
            emp.Id = Guid.NewGuid().ToString();
            emp.FullName = employee.FullName;
            emp.Salary = employee.Salary;
            emp.DOb = employee.DOb;
            emp.Address = employee.Address;
            emp.Phone = employee.Phone;
            emp.Gender = employee.Gender;
            emp.NationalId = employee.NationalId;
            emp.Nationality = employee.Nationality;
            emp.ContractDate = employee.ContractDate;
            emp.ArrivalTime = employee.ArrivalTime;
            emp.LeavingTime = employee.LeavingTime;
            emp.Extrahour = employee.Extrahour;
            emp.Penaltyhour = employee.Penaltyhour;

            if (NameExists(employee.FullName))
                throw new Exception( "There is another employee with this name" );
            if (PhoneExists(employee.Phone))
                throw new Exception("There is another employee with this phone number");
            if (NationalIdExists(employee.NationalId))
                throw new Exception("There is another employee with this national id");
            if (CompanyRoles(emp) != string.Empty)
                throw new Exception(CompanyRoles(emp));
            _context.Employees.Add(emp);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return emp;
        }

        public async Task DeleteEmployee(string employeeId)
        {   
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                throw new Exception( "NotFound" );
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> GetEmployee(string employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee Not Found");
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
           
            _context.Entry(employee).State = EntityState.Modified;
            if (employee.Id != null && !EmployeeExists(employee.Id))
            {
                throw new Exception( "NotFound" );
            }
            if (CompanyRoles(employee) != string.Empty)
                throw new Exception(CompanyRoles(employee));
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return employee;
        }

        private bool NationalIdExists(string nationalId)
        {
            return _context.Employees.Any(e => e.NationalId == nationalId);
        }
        private bool PhoneExists(string phone)
        {
            return _context.Employees.Any(e => e.Phone == phone);
        }
        private bool NameExists(string name)
        {
            return _context.Employees.Any(e => e.FullName == name);
        }

        private string CompanyRoles(Employee employee)
        {
            if (employee.LeavingTime < employee.ArrivalTime)
                return "Invalid shift hours";
            if (employee.ContractDate < new DateTime(2008, 1, 1))
                return "Invalid Contract Date";
            if (employee.DOb.AddYears(20) > employee.ContractDate)
                return "Age is less than the company threshold";
            return string.Empty;
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
