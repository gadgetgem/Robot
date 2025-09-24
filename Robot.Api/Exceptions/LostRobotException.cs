namespace Robot.Api.Exceptions
{
    public class LostRobotException : Exception
    {
        public LostRobotException(string message) : base(message) { }
    }
}
