using Robot.Api.Exceptions;
using Robot.Api.Factories;
using Robot.Api.Models;

namespace Robot.Api.Services
{
    public class MoveRobotService(IScent scent, GridValidation gridValidation, RobotValidation robotValidation) : IMoveRobotService
    {
        public async Task<List<RobotLocationResult>> ExecuteRobotCommands(CommandInput movementInstructions)
        {
            List<RobotLocationResult> results = new List<RobotLocationResult>();

            if (movementInstructions?.Robots == null || movementInstructions.GridSize == null)
            {
                results.Add(new RobotLocationResult { FinalLocation = "Invalid input provided." });
                return await Task.FromResult(results);
            }

            try
            {
                gridValidation.Validate(movementInstructions.GridSize);
            }
            catch (ArgumentException ex)
            {
                results.Add(new RobotLocationResult { FinalLocation = ex.Message });
                return await Task.FromResult(results);
            }

            foreach (RobotInput robot in movementInstructions.Robots)
            {
                if (robot?.RobotStart == null || string.IsNullOrWhiteSpace(robot.Commands))
                {
                    results.Add(new RobotLocationResult { FinalLocation = "Invalid robot input provided." });
                    continue;
                }

                try
                {
                    robotValidation.Validate(robot);
                    RobotLocationResult result = RunCommands(movementInstructions.GridSize, robot.RobotStart, robot.Commands);
                    results.Add(result);
                }
                catch (LostRobotException ex)
                {
                    results.Add(new RobotLocationResult { FinalLocation = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    results.Add(new RobotLocationResult { FinalLocation = ex.Message });
                }
            }

            return results;
        }

        private RobotLocationResult RunCommands(GridSize grid, Position start, string commands)
        {
            foreach (char command in commands)
            {
                IRobotCommand robotCommand = RobotCommandFactory.Create(command);
                robotCommand.Execute(start, grid, scent);
            }

            return new RobotLocationResult
            {
                FinalLocation = $"{start.X} {start.Y} {start.Orientation}"
            };
        }
    }
}

