using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;

public class DeleteCommentCommandException : ExceptionWithStatusCode
{
    public DeleteCommentCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public DeleteCommentCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

