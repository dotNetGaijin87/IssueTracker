using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;

public class UpdateCommentCommandException : ExceptionWithStatusCode
{
    public UpdateCommentCommandException(string message, Exception innerException)
        : base(message, innerException)  {}

    public UpdateCommentCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

