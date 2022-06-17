using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.DeleteProjectCommand;

public class DeleteProjectCommandException: ExceptionWithStatusCode
{
    public DeleteProjectCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public DeleteProjectCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

