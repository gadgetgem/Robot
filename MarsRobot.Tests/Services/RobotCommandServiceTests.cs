using MarsRobot.Api.Models;

namespace MarsRobot.Api.Services.Tests
{
    [TestFixture()]
    public class RobotCommandServiceTests
    {
        private IRobotCommandService robotCommandService;
        private RobotCommandRequest request;

        [SetUp()]
        public void Setup()
        {
            robotCommandService = new RobotCommandService();
            request = new RobotCommandRequest
            {
                Grid = new Grid { MaxX = 5, MaxY = 3 },
                Robots = new List<Robot>
                {
                    new Robot
                    {
                        StartPosition = new Position
                        {
                            X = 1,
                            Y = 1,
                            Orientation = "E"
                        },
                        Commands = "RFRFRFRF"
                    },
                    new Robot
                    {
                        StartPosition = new Position
                        {
                            X = 3,
                            Y = 2,
                            Orientation = "N"
                        },
                        Commands = "FRRFLLFFRRFLL"
                    },
                    new Robot
                    {
                        StartPosition = new Position
                        {
                            X = 0,
                            Y = 3,
                            Orientation = "W"
                        },
                        Commands = "LLFFFLFLFL"
                    }
                }
            };
        }

        [Test()]
        public void ExecuteCommands_WithValidInput_ReturnsCorrectRobotLocation()
        {
            // Act
            List<string> results = robotCommandService.ExecuteCommands(request);

            // Assert
            Assert.That(results.Count, Is.EqualTo(3));
            Assert.That(results[0], Is.EqualTo("1 1 E"));
            Assert.That(results[1], Is.EqualTo("3 3 N LOST"));
            Assert.That(results[2], Is.EqualTo("2 3 S"));
        }

        [Test()]
        public void ExecuteCommands_WithInvalidGrid_ReturnsErrorMessage()
        {
            // Arrange
            request.Grid = new Grid { MaxX = 60, MaxY = 4 };
            // Act
            List<string> results = robotCommandService.ExecuteCommands(request);
            // Assert
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.EqualTo("Grid dimensions are out of bounds"));
        }

        // Add test for x = 3 and y = 0 not lost case
        [Test()]
        public void ExecuteCommands_WhenRobotEncountersScent_DoesNotGetLost()
        {
            // Arrange
            request.Robots.Add(
               new Robot
               {
                   StartPosition = new Position { X = 3, Y = 2, Orientation = "N" },
                   Commands = "FRRFLLFFRRFLL"
               });
            // Act
            List<string> results = robotCommandService.ExecuteCommands(request);
            // Assert
            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results[3], Is.EqualTo("3 2 N"));
        }

    }
}