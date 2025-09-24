using Robot.Api.Configuration;
using Robot.Api.Models;
using Robot.Api.Services;

namespace Robot.Api.Factories
{
    public class TurnCommand : IRobotCommand
    {
        private readonly char _turnDirection;
        public TurnCommand(char turnDirection)
        {
            if (turnDirection != 'L' && turnDirection != 'R')
            {
                throw new ArgumentException("Turn direction must be 'L' or 'R'.");
            }

            _turnDirection = turnDirection;
        }
        public void Execute(Position position, GridSize grid, IScent scent)
        {
            if (!CommandDictionaries.Turns.TryGetValue(position.Orientation, out char[] orientations) || orientations == null)
            {
                throw new ArgumentException($"Invalid orientation '{position.Orientation}'.");
            }

            position.Orientation = _turnDirection == 'L' ? orientations[0] : orientations[1];
        }
    }
}
