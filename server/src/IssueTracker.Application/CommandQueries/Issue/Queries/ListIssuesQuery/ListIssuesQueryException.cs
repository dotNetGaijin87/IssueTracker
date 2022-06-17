using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;

public class ListIssuesQueryException : ExceptionWithStatusCode
{
    public ListIssuesQueryException(string message, Exception innerException)
        : base(message, innerException) {}

    public ListIssuesQueryException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

