using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;

public class UpdatePermissionCommandException : ExceptionWithStatusCode
{
    public UpdatePermissionCommandException(string message, Exception innerException)
        : base(message, innerException) {  }

    public UpdatePermissionCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

