using System.Net;

namespace IssueTracker.Application.Shared.Exceptions;

public class ExceptionWithStatusCode : Exception
{
    public ExceptionWithStatusCode() : base() { }

    public ExceptionWithStatusCode(string message) : base(message) { }

    public ExceptionWithStatusCode(string message, Exception innerException)
        : base(message, innerException) {}

    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.InternalServerError;
}