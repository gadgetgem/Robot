namespace Robot.Api.Configuration
{
    public static class CommandDictionaries
    {
        public static readonly Dictionary<char, (int dx, int dy)> Moves = new()
        {
            {'N', (0, 1)},
            {'E', (1, 0)},
            {'S', (0, -1)},
            {'W', (-1, 0)}
        };

        public static readonly Dictionary<char, char[]> Turns = new()
        {
            {'N', new[] {'W', 'E'}},
            {'E', new[] {'N', 'S'}},
            {'S', new[] {'E', 'W'}},
            {'W', new[] {'S', 'N'}}
        };
    }
}