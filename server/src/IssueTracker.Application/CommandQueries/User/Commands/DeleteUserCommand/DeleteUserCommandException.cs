using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandException : ExceptionWithStatusCode
{
    public DeleteUserCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public DeleteUserCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

