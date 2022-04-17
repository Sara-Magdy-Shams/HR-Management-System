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
    [Authorize(Policy = "HRrole")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _repo.GetUsers();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            try { return await _repo.GetUserByEmail(email); }
            catch { return NotFound(); }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{email}")]
        public async Task<IActionResult> PutUser(string email, User user)
        {
            if (email != user.Email)
            {
                return BadRequest();
            }
            try
            {
                await _repo.UpdateUser(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try { 
                await _repo.AddUser(user);
                return CreatedAtAction("GetUser", new { id = user.Email }, user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string email)
        { try
            {
                await _repo.DeleteUser(email);
                return NoContent();
            }
            catch(Exception ex)
            {
                return (ex.Message == "User Not Found")? NotFound(): BadRequest(ex.Message);
            }
        }
    }
}
