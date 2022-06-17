using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;

public class CreateProjectCommandException : ExceptionWithStatusCode
{
    public CreateProjectCommandException(string message, Exception innerException)
        : base(message, innerException) {}

    public CreateProjectCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

