using Robot.Api.Models;

namespace Robot.Api.Services
{
    public interface IMoveRobotService
    {
        public Task<List<RobotLocationResult>> ExecuteRobotCommands(CommandInput movementInstructions);
    }
}
