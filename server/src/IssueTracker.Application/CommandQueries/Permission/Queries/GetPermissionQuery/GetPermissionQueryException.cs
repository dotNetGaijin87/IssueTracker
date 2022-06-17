using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Queries.GetPermissionQuery;

public class GetPermissionQueryException : ExceptionWithStatusCode
{
    public GetPermissionQueryException(string message, Exception innerException)
        : base(message, innerException)
    { }

    public GetPermissionQueryException(string message, Exception innerException, HttpStatusCode httpStatusCode)
    : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

