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
using Microsoft.AspNetCore.Authorization;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HlidaysController : ControllerBase
    {
        private readonly IHolidayRepository _repo;

        public HlidaysController(IHolidayRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Hlidays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hliday>>> GetHlidays()
        {
            return Ok(await _repo.GetHolidays());
        }

        // GET: api/Hlidays/MotherDay
        [HttpGet("GetByName/{name}")]
        [Authorize(Policy = "ReadSettings")]

        public async Task<ActionResult<Hliday>> GetHlidayByName(string name)
        {
            try { return await _repo.GetHolidayByName(name); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // GET: api/Hlidays/2022-03-21
        [HttpGet("GetByDate/{date}")]
        [Authorize(Policy = "ReadSettings")]

        public async Task<ActionResult<Hliday>> GetHliday(DateTime date)
        {
            try { return await _repo.GetHolidayByDate(date); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        // PUT: api/Hlidays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{name}")]
        [Authorize(Policy = "UpdateSettings")]
        public async Task<ActionResult<Hliday>> PutHliday(string name, DateTime date, Hliday holiday)
        {
            try
            {
                await _repo.UpdateHoliday(name, date, holiday);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // POST: api/Hlidays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "CreateSettings")]
        public async Task<ActionResult<Hliday>> PostHliday(Hliday holiday)
        {
            try 
            {
                await _repo.AddHoliday(holiday);
                return CreatedAtAction("GetHlidayByName", new { name = holiday.Name }, holiday);
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }

        // DELETE: api/Hlidays/5
        [HttpDelete("{name}")]
        [Authorize(Policy = "DeleteSettings")]
        public async Task<IActionResult> DeleteHliday(string name)
        {
            try
            {
                await _repo.DeleteHoliday(name);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
