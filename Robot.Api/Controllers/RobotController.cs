using Microsoft.AspNetCore.Mvc;
using Robot.Api.Models;
using Robot.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Robot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotController(IMoveRobotService moveRobotService) : ControllerBase
    {
        [HttpPost("commands")]
        public async Task<IActionResult> Commands([FromBody] CommandInput robotCommands)
        {
            if (robotCommands == null)
                return BadRequest("Robot commands are required.");

            List<RobotLocationResult> results = await moveRobotService.ExecuteRobotCommands(robotCommands);
            return Ok(results);
        }
    }
}
