using Microsoft.Extensions.Options;
using Robot.Api.Configuration;

namespace Robot.Api.Models
{
    public class RobotValidation
    {
        private readonly RobotConstraintOptions _options;

        public RobotValidation(IOptions<RobotConstraintOptions> options)
        {
            _options = options.Value;
        }

        public void Validate(RobotInput robot)
        {
            if (robot.RobotStart.X > _options.MaxCoordinate || robot.RobotStart.Y > _options.MaxCoordinate)
            {
                throw new ArgumentException($"Robot start position maximum of {_options.MaxCoordinate}x{_options.MaxCoordinate}.");
            }

            if (robot.Commands.Length > _options.MaxCommandLength)
            {
                throw new ArgumentException($"Robot command length exceeds maximum of {_options.MaxCommandLength} characters.");
            }
        }
    }
}
