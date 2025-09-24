namespace MarsRobot.Api.Models
{
    public class RobotCommandRequest
    {
        public Grid Grid { get; set; }
        public List<Robot> Robots { get; set; }
    }
}