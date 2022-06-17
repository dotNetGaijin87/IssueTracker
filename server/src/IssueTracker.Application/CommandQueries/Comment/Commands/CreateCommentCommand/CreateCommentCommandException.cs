namespace IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;

public class CreateCommentCommandException : Exception
{
    public CreateCommentCommandException(string message, Exception innerException)
        : base(message, innerException)
    {  }
}

