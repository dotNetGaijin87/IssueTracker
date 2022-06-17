using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;

public class CreatePermissionCommandException : ExceptionWithStatusCode
{
    public CreatePermissionCommandException(string message, Exception innerException)
        : base(message, innerException)  { }

    public CreatePermissionCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
    : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

