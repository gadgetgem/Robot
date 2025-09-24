using Microsoft.Extensions.Options;
using Robot.Api.Configuration;

namespace Robot.Api.Models
{
    public class GridValidation
    {
        private readonly RobotConstraintOptions _options;

        public GridValidation(IOptions<RobotConstraintOptions> options)
        {
            _options = options.Value;
        }

        public void Validate(GridSize grid)
        {
            if (grid.MaxX > _options.MaxCoordinate || grid.MaxY > _options.MaxCoordinate)
            {
                throw new ArgumentException($"Grid size exceeds maximum allowed size of {_options.MaxCoordinate}x{_options.MaxCoordinate}.");
            }
        }
    }
}
