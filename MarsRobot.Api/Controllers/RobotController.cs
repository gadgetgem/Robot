using MarsRobot.Api.Models;
using MarsRobot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarsRobot.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RobotController(IRobotCommandService robotCommandService) : ControllerBase
    {
        [HttpPost]
        public IEnumerable<string> RobotCommands([FromBody] RobotCommandRequest request)
        {
            if (request == null)
            {
                return new List<string> { "Invalid command request" };
            }

            return robotCommandService.ExecuteCommands(request);
        }
    }
}
