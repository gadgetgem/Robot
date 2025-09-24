namespace Robot.Api.Services
{
    public interface IScent
    {
        bool HasScent(int x, int y);
        void AddScent(int x, int y);
    }
}
