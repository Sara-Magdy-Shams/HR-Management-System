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
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _repo;

        public GroupsController(IGroupRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<IEnumerable<Group>> GetUsers()
        {
            return await _repo.GetGroups();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(byte id)
        {
            try { return await _repo.GetGroupById(id); }
            catch (Exception exception) 
            { 
                return (exception.Message== "Group Not Found") ?NotFound(): BadRequest(exception.Message); 
            }
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(byte id, Group @group)
        {
            if (id != @group.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repo.UpdateGroup(@group);
                return NoContent();
            }
            catch (Exception exception)
            {
                return (exception.Message == "Group Not Found") ? NotFound() : BadRequest(exception.Message);
            }
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            try
            {
                await _repo.AddGroup(@group);
                return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
            }
            catch (Exception ex)
            {
                return (ex.Message=="Conflict")? Conflict(): BadRequest(ex.Message);
            }
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(byte id)
        {
            try
            {
                await _repo.DeleteGroup(id);
                return NoContent();
            }
            catch (Exception exception)
            {
                return (exception.Message == "Group Not Found") ? NotFound() : BadRequest(exception.Message);
            }
        }
    }
}
