namespace MarsRobot.Api.Models
{
    public readonly record struct ValidationResult(bool IsValid, string Message)
    {
        public static ValidationResult Success() => new ValidationResult(true, string.Empty);

        public static ValidationResult Failure(string message) => new ValidationResult(false, message);
    }
}
