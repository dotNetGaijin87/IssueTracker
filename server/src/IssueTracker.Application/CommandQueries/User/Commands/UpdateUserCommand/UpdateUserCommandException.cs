using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;

public class UpdateUserCommandException : ExceptionWithStatusCode
{
    public UpdateUserCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public UpdateUserCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

