using IssueTracker.Application.Shared.Exceptions;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;

public class GetProjectQueryException : ExceptionWithStatusCode
{
    public GetProjectQueryException(string message, Exception innerException)
        : base(message, innerException)
    { }

    public GetProjectQueryException(string message, Exception innerException, HttpStatusCode httpStatusCode)
    : base(message, innerException)
    {
        StatusCode = httpStatusCode;
    }
}

