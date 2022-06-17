using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueQuery;

public class GetIssueQueryException : ExceptionWithStatusCode
{
    public GetIssueQueryException(string message, Exception innerException)
        : base(message, innerException) {}

    public GetIssueQueryException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

