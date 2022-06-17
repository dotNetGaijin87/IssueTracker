namespace IssueTracker.Application.CommandQueries.Users.Commands.CreateUserSafelyCommand;

public class CreateUserSafelyCommandException : Exception
{
    public CreateUserSafelyCommandException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

