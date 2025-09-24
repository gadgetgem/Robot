using Robot.Api.Models;

namespace Robot.Api.Services
{
    public interface IRobotCommand
    {
        void Execute(Position position, GridSize grid, IScent scent);
    }
}
