using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;

public class UpdateProjectCommandException : ExceptionWithStatusCode
{
    public UpdateProjectCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public UpdateProjectCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

