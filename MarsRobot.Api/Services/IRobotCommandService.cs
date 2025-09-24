using MarsRobot.Api.Models;

namespace MarsRobot.Api.Services
{
    public interface IRobotCommandService
    {
        public List<string> ExecuteCommands(RobotCommandRequest request);
    }
}
