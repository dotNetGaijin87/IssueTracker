using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.DeleteIssueCommand;

public class DeleteIssueCommandException : ExceptionWithStatusCode
{
    public DeleteIssueCommandException(string message, Exception innerException)
        : base(message, innerException) { }

    public DeleteIssueCommandException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

