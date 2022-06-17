using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.DeletePermissionCommand;

public class DeletePermissionCommandException : ExceptionWithStatusCode
{
    public DeletePermissionCommandException(string message, Exception innerException)
        : base(message, innerException) {  }

    public DeletePermissionCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

