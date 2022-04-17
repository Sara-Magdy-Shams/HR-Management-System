using HRMS.DataTypes;
using HRMS.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ReadReport")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryReport _repo;
        public SalaryController(ISalaryReport repo)
        {
            _repo = repo;
        }
        // GET: api/<SalaryController>
        [HttpGet]

        public async Task<IEnumerable<Invoice>> GetAll(int month ,int year)
        {
            return await _repo.Report(month,year);
        }

        // GET api/<SalaryController>/5
        [HttpGet("{empId}")]
        public async Task<SalaryDetails> GetDetails(int month, int year, string empId)
        {
            SalaryDetails reportDetails = await _repo.SingleEmployeeReport(month, year, empId);
            return reportDetails;
        }
    }
}
