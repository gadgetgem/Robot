using Robot.Api.Configuration;
using Robot.Api.Models;

namespace Robot.Api.Services.Tests
{
    [TestFixture()]
    public class MoveRobotTests
    {
        private MoveRobotService moveRobotService;
        private GridSize gridSize;
        private List<RobotInput> robotInput;
        private CommandInput commandInput;

        [SetUp]
        public void Setup()
        {
            IScent scent = new Scent();
            RobotConstraintOptions options = new RobotConstraintOptions() { MaxCoordinate = 50, MaxCommandLength = 100 };
            GridValidation gridValidation = new GridValidation();
            RobotValidation robotValidation = new RobotValidation();

            moveRobotService = new MoveRobotService(scent, gridValidation, robotValidation);
            gridSize = new GridSize() { MaxX = 5, MaxY = 3 };

            robotInput = new List<RobotInput>()
            {
                new RobotInput()
                {
                    RobotStart = new Position() { X = 1, Y = 1, Orientation = 'E' },
                    Commands = "RFRFRFRF"
                },
                new RobotInput()
                {
                    RobotStart = new Position() { X = 3, Y = 2, Orientation = 'N' },
                    Commands = "FRRFLLFFRRFLL"
                },
                new RobotInput()
                {
                    RobotStart = new Position() { X = 0, Y = 3, Orientation = 'W' },
                    Commands = "LLFFFLFLFL"
                }
            };

            commandInput = new CommandInput()
            {
                GridSize = gridSize,
                Robots = robotInput
            };
        }

        [Test]
        public void ExecuteRobotCommands_ValidInput_ReturnsExpectedFinalPosition()
        {
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(commandInput);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0].FinalLocation, Is.EqualTo("1 1 E"));
            Assert.That(result[1].FinalLocation, Is.EqualTo("3 3 N LOST"));
            Assert.That(result[2].FinalLocation, Is.EqualTo("2 3 S"));
        }

        [Test]
        public void ExecuteRobotCommands_InvalidInput_ReturnsErrorMessage()
        {
            // Arrange
            CommandInput invalidCommandInput = new CommandInput()
            {
                GridSize = null,
                Robots = null
            };
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(invalidCommandInput);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FinalLocation, Is.EqualTo("Invalid input provided."));
        }

        [Test]
        public void ExecuteRobotCommands_GridXOutOfBounds_ReturnsErrorMessage()
        {
            // Arrange
            CommandInput outOfBoundsCommandInput = new CommandInput()
            {
                GridSize = new GridSize() { MaxX = 52, MaxY = 3 },
                Robots = robotInput
            };
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(outOfBoundsCommandInput);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FinalLocation, Is.EqualTo("Grid size exceeds maximum allowed size of 50x50."));
        }

        [Test]
        public void ExecuteRobotCommands_GridYOutOfBounds_ReturnsErrorMessage()
        {
            // Arrange
            CommandInput outOfBoundsCommandInput = new CommandInput()
            {
                GridSize = new GridSize() { MaxX = 2, MaxY = 56 },
                Robots = robotInput
            };
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(outOfBoundsCommandInput);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FinalLocation, Is.EqualTo("Grid size exceeds maximum allowed size of 50x50."));
        }

        [Test]
        public void ExecuteRobotCommands_RobotStartXOutOfBounds_ReturnsErrorMessage()
        {
            // Arrange
            List<RobotInput> robotInputOutOfBounds = new List<RobotInput>()
            {
                new RobotInput()
                {
                    RobotStart = new Position() { X = 51, Y = 1, Orientation = 'E' },
                    Commands = "RFRFRFRF"
                }
            };
            CommandInput outOfBoundsCommandInput = new CommandInput()
            {
                GridSize = gridSize,
                Robots = robotInputOutOfBounds
            };
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(outOfBoundsCommandInput);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FinalLocation, Is.EqualTo("Robot start position maximum of 50x50."));
        }

        [Test]
        public void ExecuteRobotCommands_RobotStartYOutOfBounds_ReturnsErrorMessage()
        {
            // Arrange
            List<RobotInput> robotInputOutOfBounds = new List<RobotInput>()
            {
                new RobotInput()
                {
                    RobotStart = new Position() { X = 1, Y = 60, Orientation = 'E' },
                    Commands = "RFRFRFRF"
                }
            };
            CommandInput outOfBoundsCommandInput = new CommandInput()
            {
                GridSize = gridSize,
                Robots = robotInputOutOfBounds
            };
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(outOfBoundsCommandInput);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FinalLocation, Is.EqualTo("Robot start position maximum of 50x50."));
        }

        [Test]
        public void ExecuteRobotCommands_RobotCommandLengthExceedsMaximum_ReturnsErrorMessage()
        {
            // Arrange
            List<RobotInput> robotInputLongCommands = new List<RobotInput>()
            {
                new RobotInput()
                {
                    RobotStart = new Position() { X = 1, Y = 1, Orientation = 'E' },
                    Commands = new string('F', 150)
                }
            };
            CommandInput longCommandInput = new CommandInput()
            {
                GridSize = gridSize,
                Robots = robotInputLongCommands
            };
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(longCommandInput);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FinalLocation, Is.EqualTo("Robot command length exceeds maximum of 100 characters."));
        }

        //TEST THAT 4TH rOBOT IS NOT lOST AT  (3,3,N)
        [Test]
        public void ExecuteRobotCommands_FourthRobotNotLostAtSamePosition_ReturnsExpectedFinalPosition()
        {
            // Arrange
            robotInput.Add(new RobotInput()
            {
                RobotStart = new Position() { X = 3, Y = 2, Orientation = 'N' },
                Commands = "FRRFLLFFRRFLL"
            });
            commandInput.Robots = robotInput;
            // Act
            List<RobotLocationResult> result = moveRobotService.ExecuteRobotCommands(commandInput);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[3].FinalLocation, Is.EqualTo("3 2 N"));
        }

    }
}