using System.Net;

namespace IssueTracker.Application.Shared.Exceptions;

public class UserNotAuthenticatedException : ExceptionWithStatusCode
{
    public UserNotAuthenticatedException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.Unauthorized;
    }
}