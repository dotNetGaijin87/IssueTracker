using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;

public class CreateIssueCommandException : ExceptionWithStatusCode
{
    public CreateIssueCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public CreateIssueCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

