using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Users.Queries.GetUserQuery;

public class GetUserQueryException : ExceptionWithStatusCode
{
    public GetUserQueryException(string message, Exception innerException)
        : base(message, innerException)
    { }
    public GetUserQueryException(string message, Exception innerException, HttpStatusCode httpStatusCode)
        : base(message, innerException)
    { 
        StatusCode = httpStatusCode;
    }
}

