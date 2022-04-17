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
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepository _repo;

        public SettingsController(ISettingsRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Settings
        [HttpGet]
        [Authorize(Policy = "ReadSettings")]
        public async Task<ActionResult<Setting>> GetSetting()
        {
            try
            {
                return await _repo.GetSetting();
            }
            catch (Exception ex)
            {
                return (ex.Message == "NotFound")? NotFound(): BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "CreateSettings")]
        public async Task<ActionResult<Setting>> SetSetting(Setting setting)
        {
            try
            {
                return await _repo.SetSetting(setting);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
