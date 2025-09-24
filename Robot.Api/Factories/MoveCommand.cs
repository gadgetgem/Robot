using Robot.Api.Configuration;
using Robot.Api.Exceptions;
using Robot.Api.Models;
using Robot.Api.Services;

namespace Robot.Api.Factories
{
    public class MoveCommand : IRobotCommand
    {
        public void Execute(Position position, GridSize grid, IScent scent)
        {
            int prevX = position.X;
            int prevY = position.Y;
            char prevOrientation = position.Orientation;

            if (!CommandDictionaries.Moves.TryGetValue(position.Orientation, out (int dx, int dy) move))
                throw new ArgumentException($"Invalid orientation '{position.Orientation}'.");

            position.X += move.dx;
            position.Y += move.dy;

            if (position.X < 0 || position.X > grid.MaxX || position.Y < 0 || position.Y > grid.MaxY)
            {
                if (scent.HasScent(prevX, prevY))
                {
                    position.X = prevX;
                    position.Y = prevY;
                    position.Orientation = prevOrientation;
                }
                else
                {
                    scent.AddScent(prevX, prevY);
                    throw new LostRobotException($"{prevX} {prevY} {prevOrientation} LOST");
                }
            }
        }
    }
}
