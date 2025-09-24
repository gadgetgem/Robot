namespace Robot.Api.Services
{
    public class Scent : IScent
    {
        private readonly HashSet<(int, int)> _scents = new();

        public void AddScent(int x, int y)
        {
            _scents.Add((x, y));
        }

        public bool HasScent(int x, int y)
        {
            return _scents.Contains((x, y));
        }
    }
}
