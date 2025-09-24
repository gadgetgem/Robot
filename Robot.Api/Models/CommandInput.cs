namespace Robot.Api.Models
{
    public class CommandInput
    {
        public GridSize GridSize { get; set; }

        public List<RobotInput> Robots { get; set; }
    }
}
