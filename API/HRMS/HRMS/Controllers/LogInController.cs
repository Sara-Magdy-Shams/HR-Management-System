using HRMS.ViewModels;
using HRMS.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly services.ILogger _logger;
        public LogInController(services.ILogger logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<string>> LogIn(LogInViewModel model)
        {
            try
            {
                string token = await _logger.getToken(model);
                return token;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
