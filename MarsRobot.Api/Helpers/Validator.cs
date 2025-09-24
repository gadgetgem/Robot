using MarsRobot.Api.Models;

namespace MarsRobot.Api.Helpers
{
    public class Validator
    {
        public ValidationResult ValidateGrid(Grid grid)
        {
            if (grid.MaxX < 0 || grid.MaxY < 0 || grid.MaxX > 50 || grid.MaxY > 50)
                return ValidationResult.Failure("Grid dimensions are out of bounds");

            return ValidationResult.Success();
        }

        public ValidationResult ValidateRobot(Robot robot, Grid grid)
        {
            if (robot.StartPosition.X < 0 || robot.StartPosition.X > grid.MaxX ||
                robot.StartPosition.Y < 0 || robot.StartPosition.Y > grid.MaxY)
                return ValidationResult.Failure("Robot initial position is out of bounds");

            if (!"NESW".Contains(robot.StartPosition.Orientation))
                return ValidationResult.Failure("Invalid robot orientation");

            if (robot.Commands.Length > 100)
                return ValidationResult.Failure("Robot commands exceed maximum allowed length (100)");

            if (robot.Commands.Any(c => !"LRF".Contains(c)))
                return ValidationResult.Failure("Invalid command in robot commands");

            return ValidationResult.Success();
        }
    }
}
