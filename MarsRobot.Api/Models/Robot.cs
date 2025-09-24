
namespace MarsRobot.Api.Models
{
    public class Robot
    {
        public Position StartPosition { get; set; }
        public string Commands { get; set; }

        public void Turn(char direction)
        {
            string orientations = "NESW";
            int index = orientations.IndexOf(StartPosition.Orientation[0]); // Fix: Use StartPosition.Orientation[0] to get the first character

            if (direction == 'L')
            {
                index = (index + 3) % 4; // Equivalent to -1 mod 4
            }
            else if (direction == 'R')
            {
                index = (index + 1) % 4;
            }

            StartPosition.Orientation = orientations[index].ToString(); // Fix: Convert char to string
        }

        public void MoveForward(Grid grid)
        {
            switch (StartPosition.Orientation[0])
            {
                case 'N':
                    StartPosition.Y += 1;
                    break;
                case 'E': // Fix: Use single quotes for char literals
                    StartPosition.X += 1;
                    break;
                case 'S': // Fix: Use single quotes for char literals
                    StartPosition.Y -= 1;
                    break;
                case 'W': // Fix: Use single quotes for char literals
                    StartPosition.X -= 1;
                    break;
            }
        }

    }
}
