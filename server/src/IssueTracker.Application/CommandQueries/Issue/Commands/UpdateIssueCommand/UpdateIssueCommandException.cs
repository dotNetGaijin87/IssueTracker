using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;

public class UpdateIssueCommandException : ExceptionWithStatusCode
{
    public UpdateIssueCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public UpdateIssueCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

