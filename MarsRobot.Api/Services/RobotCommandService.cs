using MarsRobot.Api.Helpers;
using MarsRobot.Api.Models;

namespace MarsRobot.Api.Services
{
    public class RobotCommandService : IRobotCommandService
    {
        private readonly HashSet<string> scentedPositions = new HashSet<string>();
        private readonly Validator validator = new Validator();

        public List<string> ExecuteCommands(RobotCommandRequest request)
        {
            ValidationResult gridValidation = validator.ValidateGrid(request.Grid);
            if (!gridValidation.IsValid)
            {
                return new List<string> { gridValidation.Message };
            }

            List<string> results = new List<string>();

            foreach (var robot in request.Robots)
            {
                ValidationResult robotValidation = validator.ValidateRobot(robot, request.Grid);
                if (!robotValidation.IsValid)
                {
                    results.Add(robotValidation.Message);
                    continue;
                }

                bool isLost = false;

                foreach (char command in robot.Commands)
                {
                    if (!"LRF".Contains(command))
                    {
                        results.Add("Invalid command in robot commands");
                        isLost = true;
                        break;
                    }

                    if (command == 'L' || command == 'R')
                    {
                        robot.Turn(command);
                    }
                    else
                    {
                        int prevX = robot.StartPosition.X;
                        int prevY = robot.StartPosition.Y;
                        string prevOrientation = robot.StartPosition.Orientation;

                        robot.MoveForward(request.Grid);

                        if (robot.StartPosition.X < 0 || robot.StartPosition.X > request.Grid.MaxX ||
                            robot.StartPosition.Y < 0 || robot.StartPosition.Y > request.Grid.MaxY)
                        {
                            isLost = HandleLostRobot(robot, prevX, prevY, prevOrientation, request.Grid, results);
                            if (isLost) break;
                        }
                    }
                }

                if (!isLost)
                {
                    results.Add($"{robot.StartPosition.X} {robot.StartPosition.Y} {robot.StartPosition.Orientation}");
                }
            }

            return results;
        }

        private bool HandleLostRobot(Robot robot, int prevX, int prevY, string prevOrientation, Grid grid, List<string> results)
        {
            string scentKey = $"{prevX} {prevY} {prevOrientation}";

            if (scentedPositions.Contains(scentKey))
            {
                robot.StartPosition.X = prevX;
                robot.StartPosition.Y = prevY;
                return false;
            }
            else
            {
                scentedPositions.Add(scentKey);

                robot.StartPosition.X = prevX;
                robot.StartPosition.Y = prevY;

                results.Add($"{robot.StartPosition.X} {robot.StartPosition.Y} {robot.StartPosition.Orientation} LOST");
                return true;
            }
        }
    }
}
