#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRMS.Models;
using HRMS.services;
using HRMS.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repo;
        public EmployeesController(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Employees
        [HttpGet]
        [Authorize(Policy = "ReadEmp")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return Ok(await _repo.GetEmployees());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        [Authorize(Policy = "ReadEmp")]
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
            var  employee = await _repo.GetEmployee(id);
            return (employee == null) ? NotFound() : employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateEmp")]
        public async Task<ActionResult<Employee>> PutEmployee(string id, Employee employee)
        {

            if (id != employee.Id)
            {
                return BadRequest();
            }
            try
            {
                return await _repo.UpdateEmployee(employee);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
        }
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "CreateEmp")]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeViewModel employee)
        {
            try
            {
                return await _repo.AddEmployee(employee);
            }
            catch (Exception ex)
            {
                return  BadRequest(ex.Message);
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteEmp")]
        public async Task<ActionResult> DeleteEmployee(string id)
        {
            try
            {
                await _repo.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
