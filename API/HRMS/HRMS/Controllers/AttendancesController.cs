using HRMS.Models;
using HRMS.services;
using HRMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceRepository _repo;
        

        public AttendancesController(IAttendanceRepository repo)
        {
            _repo = repo;
            
        }

        // GET: api/Attendances
        [HttpGet]
        [Authorize(Policy = "ReadEmp")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
            return Ok(await _repo.GetAttendances());
        }
        // GET: api/Attendances/5
        [HttpGet("FilteredAttendance")]
        [Authorize(Policy = "ReadEmp")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances(string? employeeId, DateTime StartDay, DateTime EndDay)
        {
            try
            {
                return Ok(await _repo.GetAttendanceRange(employeeId, StartDay, EndDay));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT: api/Attendances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Policy = "UpdateEmp")]
        public async Task<ActionResult<Attendance>> PutAttendance(string employeeId, DateTime Day , Attendance attendance)
        {
            try
            {
                await _repo.UpdateAttendance(employeeId,Day,attendance);  
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Attendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy ="CreateEmp")]
        public async Task<ActionResult<Attendance>> PostAttendance(AttendanceViewModel attendance)
        {
            try
            {
                return await _repo.AddAttendance(attendance);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }    
        }
        // DELETE: api/Attendances/5   
        [HttpDelete]
        [Authorize(Policy = "DeleteEmp")]
        public async Task<ActionResult> DeleteAttendance(string employeeId, DateTime Day)
        {
            try
            {
                await _repo.DeleteAttendance(employeeId, Day);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return (ex.Message=="NotFound")?NotFound("Attendance Not Found"):BadRequest(ex.Message);
            }
        }
       
    }
}
