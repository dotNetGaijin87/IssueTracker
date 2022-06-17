using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Comments.Queries.GetCommentQuery;

public class GetCommentQueryException : ExceptionWithStatusCode
{
    public GetCommentQueryException(string message, Exception innerException)
        : base(message, innerException) {}

    public GetCommentQueryException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

