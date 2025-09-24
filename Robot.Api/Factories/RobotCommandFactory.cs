using Robot.Api.Services;

namespace Robot.Api.Factories
{
    public class RobotCommandFactory
    {
        public static IRobotCommand Create(char command)
        {
            return command switch
            {
                'L' or 'R' => new TurnCommand(command),
                'F' => new MoveCommand(),
                _ => throw new ArgumentException($"Invalid command: {command}")
            };
        }
    }
}
